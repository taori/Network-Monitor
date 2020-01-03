using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetworkMonitor.Framework.Extensions;
using NetworkMonitor.Framework.Logging;
using NetworkMonitor.Models.Entities;
using NLog;

namespace NetworkMonitor.ViewModels.Services
{
	internal class TcpReceiverClient : IReceiverClient
	{
		private readonly CompositionLogger Log = new CompositionLogger();
		private static readonly ILogger LocalLogger = LogManager.GetLogger(nameof(TcpReceiverClient));

		private readonly Receiver _receiver;
		private TcpListener _tcpClient;
		private CancellationTokenSource _cts;
		private readonly List<PipeAdapter> _adapters = new List<PipeAdapter>();

		public TcpReceiverClient(Receiver receiver, IInteractiveLogger log)
		{
			Log.AddLogger(LocalLogger.Wrap());
			Log.AddLogger(log);

			_receiver = receiver;
		}

		public bool Execute()
		{
			try
			{
				_cts = new CancellationTokenSource();
				_tcpClient = new TcpListener(CreateEndpoint());
				_tcpClient.Start(120);

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
			try
			{
				while (!_cts.IsCancellationRequested)
				{
					Log.Trace("Waiting for client to connect.");
					var client = await _tcpClient.AcceptTcpClientAsync().WithCancellation(_cts.Token);
					Log.Information($"Client connected @{client.Client.RemoteEndPoint}.");
					var adapter = CreatePipeAdapter(client);
					_adapters.Add(adapter);
				}
			}
			catch (OperationCanceledException e)
			{
				Log.Trace(e.ToString());
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private PipeAdapter CreatePipeAdapter(TcpClient client)
		{
			var adapter = new PipeAdapter(client.GetStream());
			Task.Run(() => adapter.ExecuteAsync(_cts.Token));

			adapter.WhenReceived
				.Select(d => new NetworkContent(_receiver.Encoding.GetString(d.ToArray()), client.Client.RemoteEndPoint))
				.Subscribe(SequenceReceived);

			adapter.WhenTerminated
				.Select(d => (adapter, client.Client.RemoteEndPoint))
				.Subscribe(Disconnect);

			return adapter;
		}

		private void Disconnect((PipeAdapter adapter, EndPoint endPoint) tuple)
		{
			Log.Information($"Client {tuple.endPoint} disconnected.");
			_adapters.Remove(tuple.adapter);
		}

		private void SequenceReceived(NetworkContent content)
		{
			_whenReceived.OnNext(content);
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