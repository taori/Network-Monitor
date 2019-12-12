using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.ViewModel;

namespace NetworkMonitor.ViewModels.Controls
{
	public class ReceiversOverviewViewModel : ContentViewModel
	{
		protected override Task OnActivateAsync(IActivationContext context)
		{
			return Task.CompletedTask;
		}

		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield break;
		}
	}
}