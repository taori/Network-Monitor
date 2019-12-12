using NLog.Targets;

namespace NetworkMonitor.Application.Dependencies.Logging
{
	public static class LogConfiguration
	{
		public static void RegisterTargets()
		{
			Target.Register("Notification", typeof(NotificationTarget));
		}
	}
}