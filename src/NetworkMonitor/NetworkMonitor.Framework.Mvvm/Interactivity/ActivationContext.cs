using System;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;

namespace NetworkMonitor.Framework.Mvvm.Interactivity
{
	public class ActivationContext : IActivationContext
	{
		/// <inheritdoc />
		public ActivationContext(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; }

		/// <inheritdoc />
		public bool Cancelled { get; private set; }

		/// <inheritdoc />
		public void Cancel()
		{
			Cancelled = true;
		}
	}
}