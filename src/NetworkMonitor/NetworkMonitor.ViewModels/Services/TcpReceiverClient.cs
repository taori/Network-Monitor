using System;
using System.Reactive.Subjects;
using NetworkMonitor.Models.Entities;

namespace NetworkMonitor.ViewModels.Services
{
	internal class TcpReceiverClient : IReceiverClient
	{
		private readonly Receiver _receiver;

		public TcpReceiverClient(Receiver receiver)
		{
			_receiver = receiver;
		}

		public bool Execute()
		{
			return false;
		}

		public void Terminate()
		{
		}

		private Subject<NetworkContent> _whenReceived = new Subject<NetworkContent>();

		public IObservable<NetworkContent> WhenReceived => _whenReceived;
	}
}