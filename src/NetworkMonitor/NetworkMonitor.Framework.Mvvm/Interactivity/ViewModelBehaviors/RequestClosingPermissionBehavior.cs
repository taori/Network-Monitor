using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using Microsoft.Extensions.DependencyInjection;

namespace NetworkMonitor.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public class RequestClosingPermissionBehavior : InterceptClosingBehaviorBase
	{
		/// <param name="argument"></param>
		/// <inheritdoc />
		protected override async Task<bool> ShouldCancelAsync(IWindowClosingBehaviorContext argument)
		{
			var dialogService = argument.ServiceProvider.GetRequiredService<IDialogService>();
			if (await dialogService.YesNoAsync(argument.ViewModel, $"Should the window of type {argument.ViewModel.ToString()} be closed?"))
			{
				return false;
			}

			return true;
		}
	}
}