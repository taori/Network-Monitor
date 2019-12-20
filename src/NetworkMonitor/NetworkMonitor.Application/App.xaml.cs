using System;
using System.Drawing.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using NetworkMonitor.Application.Dependencies;
using NetworkMonitor.Application.Dependencies.Configuration;
using NetworkMonitor.Application.Dependencies.Logging;
using NetworkMonitor.Application.Dependencies.Setup;
using NetworkMonitor.Framework.Extensions;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;
using NetworkMonitor.Framework.Mvvm.Abstraction.Navigation;
using NetworkMonitor.Framework.Mvvm.ViewModel;
using NetworkMonitor.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Internal;

namespace NetworkMonitor.Application
{
	public partial class App : System.Windows.Application
	{
		static App()
		{
			LogConfiguration.RegisterTargets();
			Log = LogManager.GetLogger(nameof(App));
			DependencyContainer = new DependencyContainer();
		}

		private static ILogger Log { get; }

		public static readonly DependencyContainer DependencyContainer;

		private static Mutex ApplicationMutex;

		/// <inheritdoc />
		protected override void OnStartup(StartupEventArgs e)
		{
			Log.System($"{nameof(App)} - {nameof(OnStartup)}.", LogLevel.Trace);
			try
			{
				AttachAllExceptionHandlers();

				DependencyContainer.Configure();
				ShutdownMode = ShutdownMode.OnMainWindowClose;

				ApplicationMutex = new Mutex(false, typeof(App).FullName, out var mutexCreated);
				if (!mutexCreated)
				{
					Log.Warn($"Application already running. Shutting down.");
					MessageBox.Show($"Application already running. Shutting down.", "Notice", MessageBoxButton.OK, MessageBoxImage.Exclamation);
					Current.Shutdown(0);
				}

				var runners = DependencyContainer.ServiceProvider.GetServices<IConfigurationRunner>();
				foreach (var runner in runners)
				{
					Log.Debug($"Executing {nameof(IConfigurationRunner)} \"{runner.GetType().FullName}\".");
					runner.Execute();
				}

				var navigationService = DependencyContainer.ServiceProvider.GetService<INavigationService>();
				navigationService.OpenWindowAsync(new DefaultWindowViewModel(new MainViewModel()), nameof(MainViewModel));

				base.OnStartup(e);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
				throw;
			}
		}

		/// <inheritdoc />
		protected override void OnExit(ExitEventArgs e)
		{
			Log.System($"{nameof(App)} - {nameof(OnExit)}.", LogLevel.Trace);
			try
			{
				Log.Debug($"Disposing {nameof(ApplicationMutex)}.");
				ApplicationMutex?.Dispose();
				base.OnExit(e);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
				throw;
			}
		}

		/// <inheritdoc />
		protected override void OnActivated(EventArgs e)
		{
			Log.System($"{nameof(App)} - {nameof(OnActivated)}.", LogLevel.Trace);
			try
			{
				base.OnActivated(e);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
				throw;
			}
		}

		/// <inheritdoc />
		protected override void OnDeactivated(EventArgs e)
		{
			Log.System($"{nameof(App)} - {nameof(OnDeactivated)}.", LogLevel.Trace);
			try
			{
				base.OnDeactivated(e);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
				throw;
			}
		}

		/// <inheritdoc />
		protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
		{
			Log.System($"{nameof(App)} - {nameof(OnSessionEnding)}.", LogLevel.Trace);
			try
			{
				base.OnSessionEnding(e);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
				throw;
			}
		}

		/// <inheritdoc />
		protected override void OnLoadCompleted(NavigationEventArgs e)
		{
			Log.System($"{nameof(App)} - {nameof(OnLoadCompleted)}.", LogLevel.Trace);
			try
			{
				base.OnLoadCompleted(e);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
				throw;
			}
		}

		private void AttachAllExceptionHandlers()
		{
			Log.Debug("Registering exception handler for AppDomain.CurrentDomain.UnhandledException.");
			AppDomain.CurrentDomain.UnhandledException += (s, e) =>
				LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

			Log.Debug("Registering exception handler for DispatcherUnhandledException.");
			DispatcherUnhandledException += (s, e) =>
				LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");

			Log.Debug("Registering exception handler for TaskScheduler.UnobservedTaskException.");
			TaskScheduler.UnobservedTaskException += (s, e) =>
				LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
		}

		private void LogUnhandledException(Exception exception, string source)
		{
			string message = $"Unhandled exception ({source})";
			try
			{
				System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
				message = string.Format("Unhandled exception in {0} v{1}", assemblyName.Name, assemblyName.Version);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Exception in LogUnhandledException");
			}
			finally
			{
				var sb = new StringBuilder();
				sb.AppendLine(message);
				ExpandException(sb, exception);
				Log.Error(exception, sb.ToString());
			}
		}

		private void ExpandException(StringBuilder sb, Exception exception)
		{
			if (!string.IsNullOrEmpty(exception.Message))
				sb.AppendLine(exception.Message);
			if (!string.IsNullOrEmpty(exception.StackTrace))
				sb.AppendLine(exception.StackTrace);

			if (exception is AggregateException agg)
			{
				foreach (var innerException in agg.InnerExceptions)
				{
					ExpandException(sb, innerException);
				}
			}
		}
	}
}
