using System.Windows;
using NetworkMonitor.Framework.DependencyInjection;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer
{
	[InheritedMefExport(typeof(IViewComposerHook))]
	public interface IViewComposerHook
	{
		void Execute(FrameworkElement control, object dataContext);
	}
}