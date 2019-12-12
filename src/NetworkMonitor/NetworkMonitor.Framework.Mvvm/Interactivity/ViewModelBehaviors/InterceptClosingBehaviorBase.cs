using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;

namespace NetworkMonitor.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public abstract class InterceptClosingBehaviorBase : AsyncBehaviorBase<IWindowClosingBehaviorContext>
	{
		protected abstract Task<bool> ShouldCancelAsync(IWindowClosingBehaviorContext argument);

		/// <inheritdoc />
		protected override async Task OnExecuteAsync(IWindowClosingBehaviorContext context)
		{
			if (await ShouldCancelAsync(context))
				context.Cancel();
		}
	}
}