using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors
{
	public interface IViewComposedBehaviorContext : IBehaviorArgument
	{
		IViewCompositionContext CompositionContext { get; }
		IServiceContext ServiceContext { get; }
	}
}