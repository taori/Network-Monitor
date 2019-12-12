using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Data;
using NetworkMonitor.Framework.Mvvm.Extensions;
using NetworkMonitor.Framework.Mvvm.Integration.ViewMapping;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace NetworkMonitor.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	/// <summary>
	/// This behavior can be applied if you want a window to have a size restoration behavior
	/// </summary>
	public class RestoreWindowDimensionsBehavior : AsyncBehaviorBase<IViewComposedBehaviorContext>
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(RestoreWindowDimensionsBehavior));

		/// <inheritdoc />
		protected override async Task OnExecuteAsync(IViewComposedBehaviorContext context)
		{
			if (context.CompositionContext.CoordinationArguments is WindowArguments windowArguments)
			{
				var windowStorageKey = $"{nameof(WindowArguments)}.{windowArguments.WindowId}";
				if (context.ServiceContext.ServiceProvider.GetService<ISettingsStorage>() is ISettingsStorage storage)
				{
					if (storage.TryGetValue(windowStorageKey, out WindowAttributes value))
					{
						Log.Debug($"Restoring window with argument [{windowArguments.WindowId}] at [{value.Left};{value.Top}] with [{value.Width};{value.Height}]");

						if (context.CompositionContext.Control is System.Windows.Window updateWindow)
						{
							updateWindow.SetCurrentValue(System.Windows.Window.LeftProperty, value.Left);
							updateWindow.SetCurrentValue(System.Windows.Window.TopProperty, value.Top);
							updateWindow.SetCurrentValue(System.Windows.FrameworkElement.WidthProperty, value.Width);
							updateWindow.SetCurrentValue(System.Windows.FrameworkElement.HeightProperty, value.Height);
						}
					}
				}
				else
				{
					Log.Error($"No {nameof(ISettingsStorage)} available.");
					return;
				}

				if (context.CompositionContext.Control is System.Windows.Window window)
				{
					window.WhenSizeChanged()
						.Select(d => EventArgs.Empty)
						.Merge(window.WhenLocationChanged().Select(d => EventArgs.Empty))
						.Throttle(TimeSpan.FromMilliseconds(250))
						.ObserveOn(Dispatcher.CurrentDispatcher)
						.Subscribe(d =>
						{
							Log.Debug($"Updating window size information for [{windowArguments.WindowId}].");
							storage.UpdateValue(windowStorageKey, new WindowAttributes(window.Width, window.Height, window.Left, window.Top));
							storage.Save();
						});
				}
			}
		}
	}
}