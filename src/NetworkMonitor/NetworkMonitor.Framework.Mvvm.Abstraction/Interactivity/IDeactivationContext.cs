using System;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IDeactivationContext
	{
		IServiceProvider ServiceProvider { get; }

		bool Cancelled { get; }

		void Cancel();
	}
}