﻿using NLog;

namespace NetworkMonitor.Framework.Logging
{
	public static class LoggerExtensions
	{
		public static IInteractiveLogger Wrap(this ILogger source)
		{
			return new NLogInteractiveWrapper(source);
		}
	}
}