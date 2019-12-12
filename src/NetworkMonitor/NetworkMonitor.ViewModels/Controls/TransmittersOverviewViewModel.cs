using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using NetworkMonitor.Framework.Mvvm.Commands;
using NetworkMonitor.Framework.Mvvm.ViewModel;
using NetworkMonitor.ViewModels.Windows;

namespace NetworkMonitor.ViewModels.Controls
{
	public enum TransmitterType
	{
		Tcp,
		Udp,
	}
	public class TransmittersOverviewViewModel : ContentViewModel
	{
		private readonly MainViewModel _mainView;
		private readonly IDialogService _dialogService;

		public TransmittersOverviewViewModel(MainViewModel mainView, IDialogService dialogService)
		{
			_mainView = mainView;
			_dialogService = dialogService;
			NewTransmitterCommand = new TaskCommand(NewTransmitterExecute);
		}

		private Task NewTransmitterExecute(object arg)
		{
			var model = new TransmittersOverviewViewModel(_mainView, _dialogService);
			_mainView.TabController.Insert(model);
			_mainView.TabController.Focus(model);

			return Task.CompletedTask;
		}

		private ICommand _newTransmitterCommand;

		public ICommand NewTransmitterCommand
		{
			get { return _newTransmitterCommand; }
			set { SetValue(ref _newTransmitterCommand, value, nameof(NewTransmitterCommand)); }
		}

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