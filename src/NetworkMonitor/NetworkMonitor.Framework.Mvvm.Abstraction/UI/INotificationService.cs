using NetworkMonitor.Framework.DependencyInjection;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.UI
{
	[InheritedMefExport(typeof(INotificationService))]
	public interface INotificationService
	{
		void Display(INotification notification);
	}
}