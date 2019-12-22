using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetworkMonitor.Framework.Extensions;
using NetworkMonitor.Models.Entities;
using NLog;

namespace NetworkMonitor.ViewModels.Services
{
	public class PipeAdapter
	{
		private Stream _stream;
		private PipeReader _reader;
		private PipeWriter _writer;

		public PipeAdapter(Stream stream) : this(stream, new StreamPipeReaderOptions(), new StreamPipeWriterOptions())
		{
		}

		public PipeAdapter(Stream stream, StreamPipeReaderOptions readerOptions, StreamPipeWriterOptions writerOptions)
		{
			_stream = stream;
			_reader = PipeReader.Create(_stream, readerOptions);
			_writer = PipeWriter.Create(_stream, writerOptions);
		}

		public async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			await FillReader(cancellationToken);
		}

		private async Task FillReader(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				var result = await _reader.ReadAsync(cancellationToken);
				var buffer = result.Buffer;
				buffer.Slice(new SequencePosition(), buffer.Length);
			}
		}

		public void Write(ReadOnlySpan<byte> value)
		{
			_writer.Write(value);
		}

		public void Dispose()
		{
			_reader.Complete();
			_writer.Complete();
			_stream?.Dispose(); 
			_stream = null;
		}
	}

	internal class TcpReceiverClient : IReceiverClient
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(TcpReceiverClient));

		private readonly Receiver _receiver;
		private TcpListener _tcpClient;
		private CancellationTokenSource _cts;
		private readonly List<PipeAdapter> _adapters = new List<PipeAdapter>();

		public TcpReceiverClient(Receiver receiver)
		{
			_receiver = receiver;
		}

		public bool Execute()
		{
			try
			{
				_cts = new CancellationTokenSource();
				_tcpClient = new TcpListener(CreateEndpoint());
				Task.Run(ClientAcceptorLoop);
				return true;
			}
			catch (Exception e)
			{
				Log.Error(e);
				return false;
			}
		}

		private async Task ClientAcceptorLoop()
		{
			while (!_cts.IsCancellationRequested)
			{
				var client = await _tcpClient.AcceptTcpClientAsync().WithCancellation(_cts.Token);
				var adapter = CreatePipeAdapter(client);
				_adapters.Add(adapter);
			}
		}

		private PipeAdapter CreatePipeAdapter(TcpClient client)
		{
			var adapter = new PipeAdapter(client.GetStream());
			Task.Run(() => adapter.ExecuteAsync(_cts.Token));
			return adapter;
		}

		private IPEndPoint CreateEndpoint()
		{
			if (_receiver.Broadcast)
				return new IPEndPoint(IPAddress.Any, _receiver.PortNumber);

			return new IPEndPoint(IPAddress.Parse(_receiver.IpAddress), _receiver.PortNumber);
		}

		public void Terminate()
		{
			_tcpClient.Stop();
		}

		private Subject<NetworkContent> _whenReceived = new Subject<NetworkContent>();

		public IObservable<NetworkContent> WhenReceived => _whenReceived;
	}
}