using System;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors
{
	public interface IActivationBehaviorContext : IBehaviorArgument
	{
		object ViewModel { get; }
		bool Cancelled { get; }
		IServiceProvider ServiceProvider { get; }
		void Cancel();
	}
}