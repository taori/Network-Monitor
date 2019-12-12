namespace NetworkMonitor.Framework.Mvvm.Abstraction.UI
{
	public interface INotification
	{
		NotificationPosition Position { get; }

		NotificationTarget Target { get; }
	}
}