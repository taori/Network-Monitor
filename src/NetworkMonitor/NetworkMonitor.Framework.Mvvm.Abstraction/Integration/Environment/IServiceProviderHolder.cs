using System;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment
{
	public interface IServiceProviderHolder
	{
		IServiceProvider ServiceProvider { get; set; }
	}
}