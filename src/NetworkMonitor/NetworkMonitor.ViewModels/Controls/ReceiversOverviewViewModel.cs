using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using NetworkMonitor.Framework.Mvvm.Commands;
using NetworkMonitor.Framework.Mvvm.ViewModel;
using NetworkMonitor.Models.Entities;
using NetworkMonitor.Models.Providers;
using NetworkMonitor.ViewModels.Services;
using NetworkMonitor.ViewModels.Windows;

namespace NetworkMonitor.ViewModels.Controls
{
	public class ReceiversOverviewViewModel : ContentViewModel
	{
		private IApplicationSettings _applicationSettings;
		private readonly MainViewModel _mainViewModel;
		private readonly IDialogService _dialogService;
		private readonly ITabControllerManager _tabControllerManager;
		private readonly IReceiverProvider _receiverProvider;


		public ReceiversOverviewViewModel(MainViewModel mainViewModel, IDialogService dialogService, ITabControllerManager tabControllerManager, IReceiverProvider receiverProvider)
		{
			_mainViewModel = mainViewModel;
			_dialogService = dialogService;
			_tabControllerManager = tabControllerManager;
			_receiverProvider = receiverProvider;
		}

		private ICommand _newReceiverCommand;


		public ICommand NewReceiverCommand
		{
			get { return _newReceiverCommand; }
			set { SetValue(ref _newReceiverCommand, value, nameof(NewReceiverCommand)); }
		}

		private ICommand _openItemCommand;


		public ICommand OpenItemCommand
		{
			get { return _openItemCommand; }
			set { SetValue(ref _openItemCommand, value, nameof(OpenItemCommand)); }
		}

		private ICommand _deleteItemCommand;


		public ICommand DeleteItemCommand
		{
			get { return _deleteItemCommand; }
			set { SetValue(ref _deleteItemCommand, value, nameof(DeleteItemCommand)); }
		}

		private ObservableCollection<ReceiverViewModel> _receivers;

		public ObservableCollection<ReceiverViewModel> Receivers
		{
			get { return _receivers ?? new ObservableCollection<ReceiverViewModel>(); }
			set { SetValue(ref _receivers, value, nameof(Receivers)); }
		}

		protected override async Task OnActivateAsync(IActivationContext context)
		{
			NewReceiverCommand = new TaskCommand(NewReceiverExecute);
			OpenItemCommand = new TaskCommand(OpenItemExecute);
			DeleteItemCommand = new TaskCommand(DeleteItemExecute);

			var all = await _receiverProvider.GetAllAsync();
			_applicationSettings = context.ServiceProvider.GetRequiredService<IApplicationSettings>();
			Receivers = CreateReceivers(all);
		}

		private async Task DeleteItemExecute(object arg)
		{
			if (arg is ReceiverViewModel receiverViewModel)
			{
				await _receiverProvider.DeleteAsync(receiverViewModel.DataItem);
				Receivers.Remove(receiverViewModel);
			}
		}

		private Task OpenItemExecute(object arg)
		{
			if (arg is ReceiverViewModel itemViewModel)
			{
				if (_tabControllerManager.TryGetController(_mainViewModel, null, out var tabController))
				{
					var index = tabController.FindIndex(itemViewModel);
					if (index >= 0)
					{
						tabController.FocusAt(index);
					}
					else
					{
						var newIndex = tabController.Add(itemViewModel);

						if (!_applicationSettings.FocusTabOnOpen)
							return Task.CompletedTask;
						tabController.FocusAt(newIndex);
					}
				}
			}

			return Task.CompletedTask;
		}

		private ObservableCollection<ReceiverViewModel> CreateReceivers(List<Receiver> items)
		{
			var result = new ObservableCollection<ReceiverViewModel>(items.Select(d =>
			{
				var item = new ReceiverViewModel(d);
				item.WhenSaveRequested.Subscribe(SaveItem);
				return item;
			}));
			return result;
		}

		private void SaveItem(Receiver receiver)
		{
			_receiverProvider.SaveAsync(receiver);
		}

		private Task NewReceiverExecute(object arg)
		{
			var receiver = new Receiver();
			receiver.Id = Guid.NewGuid();
			receiver.DisplayName = "New receiver";
			_receiverProvider.SaveAsync(receiver);

			var viewModel = new ReceiverViewModel(receiver);
			Receivers.Add(viewModel);

			if (_applicationSettings.FocusTabOnCreate)
				OpenItemExecute(viewModel);

			return Task.CompletedTask;
		}

		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield break;
		}
	}
}