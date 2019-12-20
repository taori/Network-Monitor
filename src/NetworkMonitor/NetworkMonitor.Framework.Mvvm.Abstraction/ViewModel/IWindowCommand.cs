using System.Windows.Input;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.ViewModel
{
	public interface IWindowCommand
	{
		ICommand Command { get; set; }
	}
}