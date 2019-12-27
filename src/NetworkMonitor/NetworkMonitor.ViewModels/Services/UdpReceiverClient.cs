using System;
using System.Net;
using System.Net.Sockets;
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
	internal class UdpReceiverClient : IReceiverClient
	{
		private readonly CompositionLogger Log = new CompositionLogger();
		private static readonly ILogger LocalLogger = LogManager.GetLogger(nameof(UdpReceiverClient));

		private readonly Receiver _receiver;
		private readonly UdpClient _udpClient;
		private CancellationTokenSource _tcs;

		public UdpReceiverClient(Receiver receiver, IInteractiveLogger log)
		{
			Log.AddLogger(LocalLogger.Wrap());
			Log.AddLogger(log);

			_receiver = receiver;
			_udpClient = new UdpClient();
			_udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			_udpClient.Client.ExclusiveAddressUse = false;
			_tcs = new CancellationTokenSource();
		}

		public bool Execute()
		{
			try
			{
				if (!_receiver.IsOperational())
					return false;

				var ipEndPoint = CreateEndpointAddress();
				_udpClient.Client.Bind(ipEndPoint);
				Task.Run(ReceiverLoop);

				return true;
			}
			catch (Exception e)
			{
				Log.Error(e);
				return false;
			}
		}

		private async Task ReceiverLoop()
		{
			try
			{
				while (!_tcs.IsCancellationRequested)
				{
					var result = await _udpClient.ReceiveAsync().WithCancellation(_tcs.Token);
					var encoding = _receiver.Encoding ?? Encoding.UTF8;
					_whenReceived.OnNext(new NetworkContent(encoding.GetString(result.Buffer), result.RemoteEndPoint));
				}
			}
			catch (OperationCanceledException)
			{
			}
		}

		private IPEndPoint CreateEndpointAddress()
		{
			if(_receiver.Broadcast)
				return new IPEndPoint(IPAddress.Any, _receiver.PortNumber);

			return new IPEndPoint(IPAddress.Parse(_receiver.IpAddress), _receiver.PortNumber);
		}

		public void Terminate()
		{
			_tcs.Cancel(false);
			_udpClient?.Dispose();
		}

		private Subject<NetworkContent> _whenReceived = new Subject<NetworkContent>();
		public IObservable<NetworkContent> WhenReceived => _whenReceived;
	}
}