using System.Windows;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer
{
	public interface IViewCompositionContext
	{
		FrameworkElement Control { get; }

		object DataContext { get; }

		ICoordinationArguments CoordinationArguments { get; }
	}
}