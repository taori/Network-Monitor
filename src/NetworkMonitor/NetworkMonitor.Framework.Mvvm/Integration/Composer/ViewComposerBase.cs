using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.ViewModel;
using NetworkMonitor.Framework.Mvvm.Interactivity.ViewModelBehaviors;
using NLog;

namespace NetworkMonitor.Framework.Mvvm.Integration.Composer
{
	public abstract class ViewComposerBase : IViewComposer
	{
		public static readonly DependencyProperty CoordinationArgumentsProperty = DependencyProperty.RegisterAttached(
			"CoordinationArguments", typeof(ICoordinationArguments), typeof(ViewComposerBase), new PropertyMetadata(default(ICoordinationArguments)));

		public static void SetCoordinationArguments(DependencyObject element, ICoordinationArguments value)
		{
			element.SetValue(CoordinationArgumentsProperty, value);
		}

		public static ICoordinationArguments GetCoordinationArguments(DependencyObject element)
		{
			return (ICoordinationArguments)element.GetValue(CoordinationArgumentsProperty);
		}

		private static readonly ILogger Log = LogManager.GetLogger(nameof(ViewComposerBase));

		public IServiceContext ServiceContext { get; }
		public IEnumerable<IViewComposerHook> ComposerHooks { get; }
		public IBehaviorRunner BehaviorRunner { get; }

		protected ViewComposerBase(IServiceContext serviceContext, IEnumerable<IViewComposerHook> composerHooks, IBehaviorRunner behaviorRunner)
		{
			ServiceContext = serviceContext;
			ComposerHooks = composerHooks;
			BehaviorRunner = behaviorRunner;
		}

		/// <inheritdoc />
		public int Priority { get; }

		private TaskCompletionSource<object> DataContextLoadedCompletion = new TaskCompletionSource<object>();

		protected abstract Task FinalizeCompositionAsync(IViewCompositionContext context);

		private async void DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				if (sender is FrameworkElement element)
				{
					Log.Debug($"DataContext has changed.");
					element.DataContextChanged -= DataContextChanged;
					if (element.DataContext is IServiceProviderHolder holder)
					{
						Log.Debug($"ServiceProvider set through {nameof(IServiceProviderHolder)}.");
						holder.ServiceProvider = ServiceContext.ServiceProvider;
					}

					if (element.DataContext is IWindowViewModel windowViewModel)
					{
						if (windowViewModel.Content is IServiceProviderHolder subHolder)
							subHolder.ServiceProvider = ServiceContext.ServiceProvider;
					}

					if (element.DataContext is IDefaultBehaviorProvider behaviourProvider && element.DataContext is IBehaviorHost interactiveBehaviour)
					{
						var behaviours = behaviourProvider.GetDefaultBehaviors().ToArray();
						if (behaviours.Length > 0)
							Log.Debug($"Binding {behaviours.Length} behaviours through {nameof(IDefaultBehaviorProvider)} [{string.Join(".", behaviours.Select(s => s.GetType().ToString()))}].");

						interactiveBehaviour.Behaviors.AddRange(behaviours);
					}

					foreach (var hook in ComposerHooks)
					{
						Log.Debug($"Executing [{nameof(IViewComposerHook)}] -> [{hook.GetType().ToString()}]");
						hook.Execute(element, element.DataContext);
					}

					await BehaviorRunner.ExecuteAsync(element.DataContext as IBehaviorHost, new ActivationBehaviorContext(element.DataContext, ServiceContext.ServiceProvider));

					var coordinationArguments = GetCoordinationArguments(element);

					if (element.DataContext is ICompositionListener listener)
						listener.Execute(new ViewCompositionContext(element, element.DataContext, coordinationArguments));

					DataContextLoaded(new ViewCompositionContext(element, element.DataContext, coordinationArguments));
				}

				DataContextLoadedCompletion.TrySetResult(true);
			}
			catch (Exception exception)
			{
				DataContextLoadedCompletion.TrySetException(exception);
			}
		}

		protected virtual void DataContextLoaded(IViewCompositionContext context) { }

		/// <inheritdoc />
		public async Task<bool> ComposeAsync(IViewCompositionContext context)
		{
			try
			{
				DataContextLoadedCompletion = new TaskCompletionSource<object>();
				SetCoordinationArguments(context.Control, context.CoordinationArguments);

				Log.Debug($"Composition is being configured.");
				Configure(context);

				Log.Debug($"Attaching DataContextChanged and Unload events.");
				context.Control.DataContextChanged -= DataContextChanged;
				context.Control.DataContextChanged += DataContextChanged;

				// memory leak countermeasure
				context.Control.Unloaded += ControlOnUnloaded;

				Log.Debug($"Finalizing composition.");
				await FinalizeCompositionAsync(context);
				await BehaviorRunner.ExecuteAsync(context.DataContext as IBehaviorHost, new ViewComposedBehaviorContext(context, ServiceContext));
				await DataContextLoadedCompletion.Task;

				return true;
			}
			catch (Exception e)
			{
				Log.Error(e);
				return false;
			}
		}

		private void ControlOnUnloaded(object sender, RoutedEventArgs e)
		{
			if (sender is FrameworkElement frameworkElement)
			{
				frameworkElement.Unloaded -= ControlOnUnloaded;
				frameworkElement.DataContextChanged -= DataContextChanged;
			}
		}

		protected abstract void Configure(IViewCompositionContext context);

		/// <inheritdoc />
		public abstract bool CanHandle(FrameworkElement control);
	}
}