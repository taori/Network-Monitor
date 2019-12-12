using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.ViewModel;
using NetworkMonitor.Framework.Mvvm.Integration.ViewMapping;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace NetworkMonitor.Framework.Mvvm.ViewModel
{
	public abstract class ContentViewModel : InteractiveViewModel, IServiceProviderHolder, IContentViewModel, IDefaultBehaviorProvider
	{
		protected static readonly ILogger Log = LogManager.GetLogger(nameof(ContentViewModel));

		/// <inheritdoc />
		public async Task ActivateAsync(IActivationContext context)
		{
			await OnActivateAsync(context);
			_whenActivated.OnNext(context);
		}

		private Subject<IActivationContext> _whenActivated = new Subject<IActivationContext>();
		public IObservable<IActivationContext> WhenActivated => _whenActivated;

		protected abstract Task OnActivateAsync(IActivationContext context);

		protected async Task<bool> UpdateRegionAsync(IContentViewModel content, string regionName)
		{
			using (LoadingState.Session())
			{
				var visualizerFactory = ServiceProvider.GetRequiredService<IDisplayCoordinatorFactory>();
				var visualizer = visualizerFactory.Create(content);
				return await visualizer.DisplayAsync(content, new RegionArguments(this, regionName));
			}
		}

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; set; }

		/// <inheritdoc />
		public abstract IEnumerable<IBehavior> GetDefaultBehaviors();
	}
}