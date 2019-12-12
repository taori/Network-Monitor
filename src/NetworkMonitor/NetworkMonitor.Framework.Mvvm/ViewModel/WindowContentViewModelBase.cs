using System;
using System.Collections.Generic;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.ViewModel;

namespace NetworkMonitor.Framework.Mvvm.ViewModel
{
	public abstract class WindowContentViewModelBase : ContentViewModel, IWindowContentViewModel, IWindowSetter
	{
		private WeakReference<IWindowViewModel> _windowReference;

		/// <inheritdoc />
		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield break;
		}

		/// <inheritdoc />
		public IWindowViewModel Window => _windowReference.TryGetTarget(out var reference) ? reference : null;

		/// <inheritdoc />
		public virtual bool ClaimMainWindowOnOpen { get; }

		/// <inheritdoc />
		public abstract string GetTitle();

		/// <inheritdoc />
		public void Set(IWindowViewModel window)
		{
			_windowReference = new WeakReference<IWindowViewModel>(window);
		}
	}
}