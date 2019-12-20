using System.Collections.ObjectModel;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.ViewModel
{
	public interface IWindowContentViewModel : IContentViewModel, IDefaultBehaviorProvider
	{
		ObservableCollection<IWindowCommand> LeftWindowCommands { get; }
		ObservableCollection<IWindowCommand> RightWindowCommands { get; }
		IWindowViewModel Window { get; }
		bool ClaimMainWindowOnOpen { get; }
		string GetTitle();
	}
}