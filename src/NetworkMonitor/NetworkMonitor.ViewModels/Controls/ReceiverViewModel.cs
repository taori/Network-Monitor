using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using NetworkMonitor.Framework.Mvvm.ViewModel;

namespace NetworkMonitor.ViewModels.Controls
{
	public class ReceiverViewModel : TabViewModel
	{
		protected override Task OnActivateAsync(IActivationContext context)
		{
			Title = "New receiver";
			return Task.CompletedTask;
		}

		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield break;
		}
	}

	public class TransmitterViewModel : TabViewModel
	{
		protected override Task OnActivateAsync(IActivationContext context)
		{
			Title = "New transmitter";
			return Task.CompletedTask;
		}

		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield break;
		}
	}
}