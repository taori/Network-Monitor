using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using Microsoft.Extensions.DependencyInjection;

namespace NetworkMonitor.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public class ConfirmContentChangingBehavior : AsyncBehaviorBase<IContentChangingBehaviorContext>
	{
		/// <inheritdoc />
		protected override async Task OnExecuteAsync(IContentChangingBehaviorContext context)
		{
			if (!await context.ServiceProvider.GetRequiredService<IDialogService>().YesNoAsync(context.OldViewModel, "Change content?"))
				context.Cancel();
		}
	}
}