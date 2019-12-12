using System.Windows;
using NetworkMonitor.Framework.DependencyInjection;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer
{
	[InheritedMefExport(typeof(IViewComposerFactory))]
	public interface IViewComposerFactory
	{
		IViewComposer Create(FrameworkElement control);
	}
}