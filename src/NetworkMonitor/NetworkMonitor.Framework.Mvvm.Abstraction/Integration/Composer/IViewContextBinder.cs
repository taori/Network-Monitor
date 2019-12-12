
using NetworkMonitor.Framework.DependencyInjection;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer
{
	[InheritedMefExport(typeof(IViewContextBinder))]
	public interface IViewContextBinder
	{
		bool TryBind(IViewCompositionContext context);
	}
}