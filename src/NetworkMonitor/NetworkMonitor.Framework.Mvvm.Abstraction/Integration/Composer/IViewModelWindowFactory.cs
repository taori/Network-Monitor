using System.Windows;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer
{
	public interface IViewModelWindowFactory
	{
		bool CanCreateWindow(object dataContext);
		Window CreateWindow(object dataContext);
	}
}