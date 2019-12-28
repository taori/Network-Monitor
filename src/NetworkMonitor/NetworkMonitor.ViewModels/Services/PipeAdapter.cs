using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkMonitor.ViewModels.Services
{
	public class PipeAdapter
	{
		private Stream _stream;
		private PipeReader _reader;
		private PipeWriter _writer;

		private Subject<ReadOnlySequence<byte>> _whenReceived = new Subject<ReadOnlySequence<byte>>();
		public IObservable<ReadOnlySequence<byte>> WhenReceived => _whenReceived;

		private Subject<PipeAdapter> _whenTerminated = new Subject<PipeAdapter>();
		public IObservable<PipeAdapter> WhenTerminated => _whenTerminated;

		public PipeAdapter(Stream stream) : this(stream, new StreamPipeReaderOptions(leaveOpen: true), new StreamPipeWriterOptions(leaveOpen: true))
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
			_whenTerminated.OnNext(this);
		}

		private async Task FillReader(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				var result = await _reader.ReadAsync(cancellationToken);
				if (result.IsCanceled || result.IsCompleted)
					return;

				_whenReceived.OnNext(result.Buffer);
				_reader.AdvanceTo(result.Buffer.End);
			}
		}

		public async Task WriteAsync(ReadOnlyMemory<byte> value)
		{
			await _writer.WriteAsync(value);
		}

		public void Dispose()
		{
			_whenTerminated.OnNext(null);
			_whenReceived.OnCompleted();
			_whenTerminated.OnCompleted();
			_reader.Complete();
			_writer.Complete();
			_stream?.Dispose();
			_stream = null;
		}
	}
}