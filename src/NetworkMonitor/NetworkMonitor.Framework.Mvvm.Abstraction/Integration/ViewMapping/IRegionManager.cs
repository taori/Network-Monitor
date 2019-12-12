using System.Windows;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping
{
	public interface IRegionManager
	{
		FrameworkElement GetControl(object regionViewModelHolder, string regionName);
		FrameworkElement GetHostingWindow(object viewModel);
	}
}