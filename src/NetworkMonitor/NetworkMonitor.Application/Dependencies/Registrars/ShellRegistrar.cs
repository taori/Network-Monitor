using System;
using System.Text.RegularExpressions;
using NetworkMonitor.Application.Dependencies.Setup;
using NetworkMonitor.Application.Dependencies.UI;
using NetworkMonitor.Framework.DependencyInjection;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.Navigation;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using NetworkMonitor.Framework.Mvvm.Integration.Environment;
using NetworkMonitor.Framework.Mvvm.Integration.ViewMapping;
using NetworkMonitor.Framework.Mvvm.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Navigation;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace NetworkMonitor.Application.Dependencies.Registrars
{
	public class ShellRegistrar : IServiceRegistrar
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(ShellRegistrar));

		/// <inheritdoc />
		public void Register(IServiceCollection services)
		{
			Singleton<IDialogService, MetroDialogService>(services);
			Singleton<INavigationService, NavigationService>(services);
			Singleton<IInjectionAssemblyLoader, InjectionAssemblyLoader>(services);
			Singleton<IRegionManager, RegionManager>(services);
			Singleton<IWindowManager, WindowManager>(services);
			Singleton<IDisplayCoordinatorFactory, DisplayCoordinatorFactory>(services);
			Singleton<IBehaviorRunner, BehaviorRunner>(services);
			Singleton<ISettingsStorage, SettingsStorage>(services);
			Singleton<ITabControllerManager, TabManager>(services);

			Transient<IServiceContext, ServiceContext>(services);
			Transient<IViewModelWindowFactory, WindowFactory>(services);
			services.AddTransient<IRegexDataTemplatePatternProvider>(CreateDefaultConventionPattern);
		}

		private void Singleton<TService, TImplementation>(IServiceCollection services) where TService : class where TImplementation : class, TService
		{
			Log.Debug($"Registering [Singleton] [{typeof(TImplementation)}] -> [{typeof(TService)}].");
			services.AddSingleton<TService, TImplementation>();
		}
		private void Transient<TService, TImplementation>(IServiceCollection services) where TService : class where TImplementation : class, TService
		{
			Log.Debug($"Registering [Transient] [{typeof(TImplementation)}] -> [{typeof(TService)}].");
			services.AddTransient<TService, TImplementation>();
		}

		private static InlineMvvmPattern CreateDefaultConventionPattern(IServiceProvider provider)
		{
			return new InlineMvvmPattern(
				new Regex("(?<ns1>NetworkMonitor\\.)(?<ignore>ViewModels)(?<ns2>.*?)(?<class>\\.[^.]+)(?=ViewModel$)", RegexOptions.Compiled, TimeSpan.FromMilliseconds(30)),
				new Regex("(?<ns1>NetworkMonitor\\.)(?<ignore>Application\\.Views)(?<ns2>.*?)(?<class>\\.[^.]+)(?=View$|Page$|Control$|Window$)", RegexOptions.Compiled, TimeSpan.FromMilliseconds(30))
			);
		}
	}
}