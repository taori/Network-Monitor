using NetworkMonitor.Framework.Mvvm.Abstraction.UI;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping
{
	public interface ITabControllerManager
	{
		bool TryGetController(object viewModel, string name, out ITabController controller);
	}
}