using System;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors
{
	public interface IWindowClosedBehaviorContext : IBehaviorArgument
	{
		object ViewModel { get; }

		IServiceProvider ServiceProvider { get; }

		IViewCompositionContext Context { get; }
	}
}