using System;
using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using NetworkMonitor.Framework.Mvvm.Abstraction.Navigation;
using NetworkMonitor.Framework.Mvvm.Abstraction.ViewModel;
using NetworkMonitor.Framework.Mvvm.Integration.ViewMapping;
using NLog;

namespace NetworkMonitor.Framework.Mvvm.Navigation
{
	public class NavigationService : INavigationService
	{
		public IDisplayCoordinatorFactory CoordinatorFactory { get; }

		private static readonly ILogger Log = LogManager.GetLogger(nameof(NavigationService));

		public NavigationService(IDisplayCoordinatorFactory coordinatorFactory)
		{
			CoordinatorFactory = coordinatorFactory;
		}

		/// <inheritdoc />
		public async Task<bool> OpenWindowAsync(IWindowViewModel viewModel, string windowId)
		{
			var coordinator = CoordinatorFactory.Create(viewModel);
			Log.Debug($"Opening window {viewModel.GetType().FullName}");
			return await coordinator.DisplayAsync(viewModel, new WindowArguments(windowId ?? Guid.NewGuid().ToString()));
		}
	}
}