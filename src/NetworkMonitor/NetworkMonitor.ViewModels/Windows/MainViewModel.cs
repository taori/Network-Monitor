using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Amusoft.UI.WPF.Adorners;
using Amusoft.UI.WPF.Notifications;
using NetworkMonitor.Framework.Extensions;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.Navigation;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using NetworkMonitor.Framework.Mvvm.Commands;
using NetworkMonitor.Framework.Mvvm.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.UI;
using NetworkMonitor.Framework.Mvvm.ViewModel;
using NetworkMonitor.ViewModels.Common;
using NetworkMonitor.ViewModels.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace NetworkMonitor.ViewModels.Windows
{
	public class MainViewModel : WindowContentViewModelBase
	{
		protected override async Task OnActivateAsync(IActivationContext context)
		{
			Transmitters = new TransmittersOverviewViewModel(this, context.ServiceProvider.GetRequiredService<IDialogService>());
			Receivers = new ReceiversOverviewViewModel();

			await Task.WhenAll(Transmitters.ActivateAsync(context), Receivers.ActivateAsync(context));
		}

		private TransmittersOverviewViewModel _transmitters;

		public TransmittersOverviewViewModel Transmitters
		{
			get { return _transmitters; }
			set { SetValue(ref _transmitters, value, nameof(Transmitters)); }
		}

		private ReceiversOverviewViewModel _receivers;

		public ReceiversOverviewViewModel Receivers
		{
			get { return _receivers; }
			set { SetValue(ref _receivers, value, nameof(Receivers)); }
		}

		private ITabController _tabController;

		public ITabController TabController
		{
			get { return _tabController; }
			set { SetValue(ref _tabController, value, nameof(TabController)); }
		}

		/// <inheritdoc />
		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield return new RestoreWindowDimensionsBehavior();
		}

		/// <inheritdoc />
		public override string GetTitle()
		{
			return "Network Monitor";
		}
	}
}