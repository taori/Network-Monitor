using System;
using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;

namespace NetworkMonitor.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public class DisposeOnCloseBehavior : AsyncBehaviorBase<IWindowClosedBehaviorContext>
	{
		/// <inheritdoc />
		protected override Task OnExecuteAsync(IWindowClosedBehaviorContext context)
		{
			if (context.ViewModel is IDisposable disposable)
				disposable.Dispose();

			return Task.CompletedTask;
		}
	}
}