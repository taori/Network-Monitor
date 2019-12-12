using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.ViewModel;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Navigation
{
	public interface INavigationService
	{
		Task<bool> OpenWindowAsync(IWindowViewModel viewModel, string windowId);
	}
}