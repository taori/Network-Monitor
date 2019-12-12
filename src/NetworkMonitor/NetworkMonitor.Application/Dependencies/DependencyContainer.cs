using System;
using NetworkMonitor.Application.Dependencies.Setup;
using NetworkMonitor.Framework.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace NetworkMonitor.Application.Dependencies
{
	public class DependencyContainer
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(DependencyContainer));

		private readonly IServiceCollection _serviceCollection = new ServiceCollection();

		public void Configure()
		{
			Log.Debug("Registering manuel services.");
			ManualRegisters(_serviceCollection);

			Log.Debug("Discovering registrars.");
			_serviceCollection.DiscoverRegistrars(_serviceCollection.BuildServiceProvider(true).CreateScope().ServiceProvider);

			Log.Debug("Building service provider.");
			_serviceCollection.AddSingleton<IServiceProvider>(_serviceCollection.BuildServiceProvider(true));
			var serviceProvider = _serviceCollection.BuildServiceProvider(true);

			Log.Debug("Creating service provider.");
			var serviceScope = serviceProvider.CreateScope();

			Log.Debug("Assigning service provider.");
			ServiceProvider = serviceScope.ServiceProvider;
		}

		private void ManualRegisters(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IInjectionAssemblyLoader, InjectionAssemblyLoader>();
		}

		public IServiceProvider ServiceProvider { get; private set; }
	}
}