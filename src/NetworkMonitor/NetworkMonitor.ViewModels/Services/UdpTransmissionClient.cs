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
	internal class UdpTransmissionClient : ITransmissionClient
	{
		private readonly Transmitter _transmitter;

		private readonly CompositionLogger Log = new CompositionLogger();
		private static readonly ILogger LocalLogger = LogManager.GetLogger(nameof(UdpTransmissionClient));

		public UdpTransmissionClient(Transmitter transmitter, IInteractiveLogger log)
		{
			Log.AddLogger(LocalLogger.Wrap());
			Log.AddLogger(log);

			_transmitter = transmitter;
			_client = new UdpClient();
		}

		private UdpClient _client;

		private Subject<string> _whenReceived = new Subject<string>();
		public IObservable<string> WhenReceived => _whenReceived;

		public async Task<int> SendAsync(byte[] bytes)
		{
			var endPoint = GetEndpoint();
			return await _client.SendAsync(bytes, bytes.Length, endPoint).ConfigureAwait(false);
		}

		private IPEndPoint GetEndpoint()
		{
			if (_transmitter.Broadcast)
			{
				return new IPEndPoint(IPAddress.Broadcast, _transmitter.PortNumber);
			}
			else
			{
				return new IPEndPoint(IPAddress.Parse(_transmitter.IpAddress), _transmitter.PortNumber);
			}
		}

		public bool Execute()
		{
			return _transmitter.IsOperational();
		}

		public void Terminate()
		{
			_client.Dispose();
		}
	}
}