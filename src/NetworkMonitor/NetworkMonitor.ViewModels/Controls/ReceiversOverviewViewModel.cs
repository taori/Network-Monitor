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
	public class ReceiversOverviewViewModel : ContentViewModel
	{
		private readonly MainViewModel _mainViewModel;
		private readonly IDialogService _dialogService;
		private readonly ITabControllerManager _tabControllerManager;

		public ReceiversOverviewViewModel(MainViewModel mainViewModel, IDialogService dialogService, ITabControllerManager tabControllerManager)
		{
			_mainViewModel = mainViewModel;
			_dialogService = dialogService;
			_tabControllerManager = tabControllerManager;
		}

		private ICommand _newReceiverCommand;

		public ICommand NewReceiverCommand
		{
			get { return _newReceiverCommand; }
			set { SetValue(ref _newReceiverCommand, value, nameof(NewReceiverCommand)); }
		}

		protected override Task OnActivateAsync(IActivationContext context)
		{
			NewReceiverCommand = new TaskCommand(NewReceiverExecute);
			return Task.CompletedTask;
		}

		private Task NewReceiverExecute(object arg)
		{
			if (_tabControllerManager.TryGetController(_mainViewModel, null, out var tabController))
			{
				tabController.Add(new ReceiverViewModel());
				tabController.FocusLast();
			}

			return Task.CompletedTask;
		}

		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield break;
		}
	}
}