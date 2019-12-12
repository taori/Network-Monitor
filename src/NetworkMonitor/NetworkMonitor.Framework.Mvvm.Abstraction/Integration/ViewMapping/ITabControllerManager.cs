using NetworkMonitor.Framework.Mvvm.Abstraction.UI;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping
{
	public interface ITabControllerManager
	{
		ITabController GetController(object viewModel, string name);
	}
}