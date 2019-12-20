using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using NetworkMonitor.Models.Entities;

namespace NetworkMonitor.ViewModels.Services
{
	internal class TcpTransmissionClient : ITransmissionClient
	{
		private readonly Transmitter _transmitter;

		public TcpTransmissionClient(Transmitter transmitter)
		{
			_transmitter = transmitter;
		}

		public bool Execute()
		{
			return false;
		}

		public void Terminate()
		{
		}

		public Task<int> SendAsync(byte[] bytes)
		{
			return Task.FromResult(0);
		}
	}
}