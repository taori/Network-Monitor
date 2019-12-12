using System;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment
{
	public interface IServiceContext
	{
		IServiceProvider ServiceProvider { get; }
	}
}