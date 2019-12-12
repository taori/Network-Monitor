using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using NetworkMonitor.Framework.Mvvm.Abstraction.ViewModel;
using NetworkMonitor.Framework.Mvvm.Extensions;
using NetworkMonitor.Framework.Mvvm.Interactivity.Window;
using NLog;

namespace NetworkMonitor.Framework.Mvvm.Integration.Composer
{
	public class WindowComposer : ViewComposerBase
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(WindowComposer));

		/// <inheritdoc />
		public WindowComposer(IServiceContext serviceContext, IEnumerable<IViewComposerHook> composerHooks, IBehaviorRunner behaviorRunner) : base(serviceContext, composerHooks, behaviorRunner)
		{
		}

		/// <param name="context"></param>
		/// <inheritdoc />
		protected override Task FinalizeCompositionAsync(IViewCompositionContext context)
		{
			if (context.Control is Window window)
			{
				context.Control.DataContext = context.DataContext;

				if (context.DataContext is IWindowViewModel windowViewModel && windowViewModel.Content.ClaimMainWindowOnOpen)
					Application.Current.MainWindow = window;

				window.Show();
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override void Configure(IViewCompositionContext context)
		{
			if (context.Control is Window window)
			{
				if (context.DataContext is IWindowViewModel windowViewModel)
				{
					window.SizeToContent = windowViewModel.SizeToContent;
					window.ShowInTaskbar = windowViewModel.ShowInTaskbar;

					windowViewModel.WhenFocusRequested.Subscribe(o => window.Focus());
					windowViewModel.WhenClosingRequested.Subscribe(o => window.Close());
					windowViewModel.WhenMinimizeRequested.Subscribe(o => window.WindowState = WindowState.Minimized);
					windowViewModel.WhenMaximizeRequested.Subscribe(o => window.WindowState = WindowState.Maximized);
					windowViewModel.WhenNormalizeRequested.Subscribe(o => window.WindowState = WindowState.Normal);

					window.WhenClosing().Subscribe(args =>
					{
						if (windowViewModel is IWindowListener listener)
							listener.NotifyClosing(args);
					});

					window.WhenStateChanged().Throttle(TimeSpan.FromMilliseconds(50)).Subscribe(args =>
					{
						if (windowViewModel is IWindowListener listener)
							listener.NotifyWindowStateChange(args);
					});

					window.WhenLocationChanged().Throttle(TimeSpan.FromMilliseconds(50)).Subscribe(args =>
					{
						if (windowViewModel is IWindowListener listener)
							listener.NotifyLocationChanged(args);
					});

					window.WhenSizeChanged().Throttle(TimeSpan.FromMilliseconds(50)).Subscribe(args =>
					{
						if (windowViewModel is IWindowListener listener)
							listener.NotifySizeChanged(args);
					});

					window.WhenClosed().Subscribe(async (args) =>
					{
						if (windowViewModel is IWindowListener listener)
							listener.NotifyClosed();

						await BehaviorRunner.ExecuteAsync(windowViewModel as IBehaviorHost, new WindowClosedContext(windowViewModel, ServiceContext.ServiceProvider, context));
					});
				}
			}
		}

		/// <inheritdoc />
		public override bool CanHandle(FrameworkElement control)
		{
			return control is Window;
		}
	}
}