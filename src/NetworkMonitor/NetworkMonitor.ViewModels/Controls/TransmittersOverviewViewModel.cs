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
	public class TransmittersOverviewViewModel : TabViewModel
	{
		private IApplicationSettings _applicationSettings;
		private readonly MainViewModel _mainView;
		private readonly IDialogService _dialogService;
		private readonly ITabControllerManager _tabControllerManager;
		private readonly ITransmitterProvider _transmitterProvider;


		public TransmittersOverviewViewModel(MainViewModel mainView, IDialogService dialogService,
			ITabControllerManager tabControllerManager, ITransmitterProvider transmitterProvider)
		{
			_mainView = mainView;
			_dialogService = dialogService;
			_tabControllerManager = tabControllerManager;
			_transmitterProvider = transmitterProvider;
			NewTransmitterCommand = new TaskCommand(NewTransmitterExecute);
			OpenItemCommand = new TaskCommand(OpenItemExecute);
			DeleteItemCommand = new TaskCommand(DeleteItemExecute);
		}

		private async Task DeleteItemExecute(object arg)
		{
			if (arg is TransmitterViewModel itemViewModel)
			{
				await _transmitterProvider.DeleteAsync(itemViewModel.DataItem);
				Transmitters.Remove(itemViewModel);
			}
		}

		private Task OpenItemExecute(object arg)
		{
			if (arg is TransmitterViewModel itemViewModel)
			{
				if (_tabControllerManager.TryGetController(_mainView, null, out var tabController))
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

		private Task NewTransmitterExecute(object arg)
		{
			var transmitter = new Transmitter();
			transmitter.Id = Guid.NewGuid();
			transmitter.DisplayName = "New transmitter";
			_transmitterProvider.SaveAsync(transmitter);

			var viewModel = new TransmitterViewModel(transmitter, _dialogService, _mainView);
			Transmitters.Add(viewModel);

			if (_applicationSettings.FocusTabOnCreate)
				OpenItemExecute(viewModel);

			return Task.CompletedTask;
		}

		private ICommand _deleteItemCommand;


		public ICommand DeleteItemCommand
		{
			get { return _deleteItemCommand; }
			set { SetValue(ref _deleteItemCommand, value, nameof(DeleteItemCommand)); }
		}

		private ICommand _openItemCommand;


		public ICommand OpenItemCommand
		{
			get { return _openItemCommand; }
			set { SetValue(ref _openItemCommand, value, nameof(OpenItemCommand)); }
		}

		private ObservableCollection<TransmitterViewModel> _transmitters;


		public ObservableCollection<TransmitterViewModel> Transmitters
		{
			get { return _transmitters ?? new ObservableCollection<TransmitterViewModel>(); }
			set { SetValue(ref _transmitters, value, nameof(Transmitters)); }
		}

		private ICommand _newTransmitterCommand;

		public ICommand NewTransmitterCommand
		{
			get { return _newTransmitterCommand; }
			set { SetValue(ref _newTransmitterCommand, value, nameof(NewTransmitterCommand)); }
		}

		protected override async Task OnActivateAsync(IActivationContext context)
		{
			var all = await _transmitterProvider.GetAllAsync();
			_applicationSettings = context.ServiceProvider.GetRequiredService<IApplicationSettings>();
			Transmitters = CreateTransmitters(all);
		}

		private ObservableCollection<TransmitterViewModel> CreateTransmitters(List<Transmitter> all)
		{
			return new ObservableCollection<TransmitterViewModel>(all.Select(d =>
			{
				var viewModel = new TransmitterViewModel(d, _dialogService, _mainView);
				viewModel.WhenSaveRequested.Subscribe(SaveItem);
				return viewModel;
			}));
		}

		private async void SaveItem(Transmitter transmitter)
		{
			await _transmitterProvider.SaveAsync(transmitter);
		}

		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield break;
		}
	}
}