using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using NetworkMonitor.Framework.Logging;
using NetworkMonitor.Models.Entities;
using NLog;

namespace NetworkMonitor.ViewModels.Services
{
	internal class TcpTransmissionClient : ITransmissionClient
	{
		private readonly CompositionLogger Log = new CompositionLogger();
		private static readonly ILogger LocalLogger = LogManager.GetLogger(nameof(TcpTransmissionClient));
		
		private readonly Transmitter _transmitter;
		private TcpClient _tcpClient;
		private PipeAdapter _adapter;

		public TcpTransmissionClient(Transmitter transmitter, IInteractiveLogger log)
		{
			Log.AddLogger(LocalLogger.Wrap());
			Log.AddLogger(log);

			_transmitter = transmitter;
		}

		public bool Execute()
		{
			try
			{
				_tcpClient = new TcpClient();
				_tcpClient.Connect(CreateEndpoint());
				_adapter = new PipeAdapter(_tcpClient.GetStream());
				return true;
			}
			catch (Exception e)
			{
				Log.Error(e);
				return false;
			}
		}

		private IPEndPoint CreateEndpoint()
		{
			if (_transmitter.Broadcast)
				return new IPEndPoint(IPAddress.Any, _transmitter.PortNumber);

			return new IPEndPoint(IPAddress.Parse(_transmitter.IpAddress), _transmitter.PortNumber);
		}

		public void Terminate()
		{
			_adapter?.Dispose();
			_tcpClient?.Close();
			_tcpClient?.Dispose();
			_tcpClient = null;
		}

		public Task<int> SendAsync(byte[] bytes)
		{
			_adapter.Write(bytes);
			return Task.FromResult(bytes.Length);
		}
	}
}