using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetworkMonitor.Models.Entities;
using NLog;

namespace NetworkMonitor.ViewModels.Services
{
	internal class UdpReceiverClient : IReceiverClient
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(UdpReceiverClient));

		private readonly Receiver _receiver;
		private readonly UdpClient _udpClient;
		private CancellationTokenSource _tcs;

		public UdpReceiverClient(Receiver receiver)
		{
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
			while (!_tcs.IsCancellationRequested)
			{
				var result = await _udpClient.ReceiveAsync();
				var encoding = _receiver.Encoding ?? Encoding.UTF8;
				_whenReceived.OnNext(encoding.GetString(result.Buffer));
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
			_udpClient?.Dispose();
		}

		private Subject<string> _whenReceived = new Subject<string>();
		public IObservable<string> WhenReceived => _whenReceived;
	}
}