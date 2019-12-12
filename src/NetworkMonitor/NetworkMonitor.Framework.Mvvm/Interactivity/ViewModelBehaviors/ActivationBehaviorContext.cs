using System;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;

namespace NetworkMonitor.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public class ActivationBehaviorContext : IActivationBehaviorContext
	{
		/// <inheritdoc />
		public ActivationBehaviorContext(object viewModel, IServiceProvider serviceProvider)
		{
			ViewModel = viewModel;
			ServiceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public object ViewModel { get; }

		/// <inheritdoc />
		public bool Cancelled { get; private set; }

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; }

		/// <inheritdoc />
		public void Cancel()
		{
			Cancelled = true;
		}
	}
}