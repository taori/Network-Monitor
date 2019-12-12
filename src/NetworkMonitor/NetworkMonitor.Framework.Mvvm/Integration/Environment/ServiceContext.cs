using System;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;

namespace NetworkMonitor.Framework.Mvvm.Integration.Environment
{
	public class ServiceContext : IServiceContext
	{
		/// <inheritdoc />
		public ServiceContext(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; }
	}
}