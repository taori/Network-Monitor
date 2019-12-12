using System.Collections.Generic;
using System.Reactive.Disposables;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Interactivity;

namespace NetworkMonitor.Framework.Mvvm.ViewModel
{
	public abstract class InteractiveViewModel : ViewModelBase, IBehaviorHost, IBusyStateHolder
	{
		protected readonly CompositeDisposable Disposables = new CompositeDisposable();

		public List<IBehavior> Behaviors { get; } = new List<IBehavior>();

		/// <inheritdoc />
		public IBusyState LoadingState { get; } = new BusyState();

		private bool _disposed = false;

		protected virtual void Dispose(bool managedDispose)
		{
			if (!_disposed)
			{
				if (managedDispose)
				{
					foreach (var behaviour in Behaviors)
					{
						behaviour.Dispose();
					}
					Disposables.Dispose();
				}

				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}