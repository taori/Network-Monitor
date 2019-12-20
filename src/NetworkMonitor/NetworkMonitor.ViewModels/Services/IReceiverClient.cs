using System;

namespace NetworkMonitor.ViewModels.Services
{
	public interface IReceiverClient
	{
		bool Execute();
		void Terminate();
		IObservable<string> WhenReceived { get; }
	}
}