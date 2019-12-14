using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using NetworkMonitor.Framework.Mvvm.Commands;
using NetworkMonitor.Framework.Mvvm.ViewModel;
using NetworkMonitor.ViewModels.Windows;

namespace NetworkMonitor.ViewModels.Controls
{
	public class TransmittersOverviewViewModel : TabViewModel
	{
		private readonly MainViewModel _mainView;
		private readonly IDialogService _dialogService;
		private readonly ITabControllerManager _tabControllerManager;

		public TransmittersOverviewViewModel(MainViewModel mainView, IDialogService dialogService, ITabControllerManager tabControllerManager)
		{
			_mainView = mainView;
			_dialogService = dialogService;
			_tabControllerManager = tabControllerManager;
			NewTransmitterCommand = new TaskCommand(NewTransmitterExecute);
		}

		private Task NewTransmitterExecute(object arg)
		{
			var model = new TransmitterViewModel();
			if (_tabControllerManager.TryGetController(_mainView, null, out var controller))
			{
				model.Closable = true;
				controller.Add(model);
				controller.Focus(model);
			}

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