using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.ViewModel
{
	public interface IWindowContentViewModel : IContentViewModel, IDefaultBehaviorProvider
	{
		IWindowViewModel Window { get; }
		bool ClaimMainWindowOnOpen { get; }
		string GetTitle();
	}
}