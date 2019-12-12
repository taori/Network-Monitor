using System.ComponentModel;
using System.Windows;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.UI
{
	public interface IWindowListener
	{
		void NotifyClosed();
		void NotifyClosing(CancelEventArgs args);
		void NotifyWindowStateChange(WindowState args);
		void NotifyLocationChanged(Point args);
		void NotifySizeChanged(SizeChangedEventArgs args);
	}
}