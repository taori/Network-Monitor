using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.ViewModel;
using NetworkMonitor.Models.Entities;
using NetworkMonitor.ViewModels.Common;
using TransmitterType = NetworkMonitor.Models.Enums.TransmitterType;

namespace NetworkMonitor.ViewModels.Controls
{
	public class TransmitterViewModel : TabViewModel
	{
		public readonly Transmitter DataItem;

		public TransmitterViewModel(Transmitter dataItem)
		{
			DataItem = dataItem;
			Closable = true;
			DisplayName = dataItem.DisplayName;
			PortNumber = dataItem.PortNumber;
			TransmitterType = dataItem.TransmitterType;
		}

		public TransmitterViewModel()
		{
		}

		protected override Task OnActivateAsync(IActivationContext context)
		{
			Title = DisplayName;
			SaveCommand = new ActionCommand(SaveExecute);
			ActivateCommand = new ActionCommand(ActivateExecute);
			NewMessageCommand = new ActionCommand(NewMessageExecute);
			ClearLogCommand = new ActionCommand(ClearLogExecute);
			return Task.CompletedTask;
		}

		private void ClearLogExecute()
		{
			Messages.Clear();
		}

		private void NewMessageExecute()
		{
			Messages.Insert(0, new NetworkContentMessage(NewMessage));
			Messages.Insert(0, new NetworkStatusMessage(NetworkStatusMessageType.Connection, NewMessage));
			Messages.Insert(0, new NetworkStatusMessage(NetworkStatusMessageType.Information, NewMessage));
			Messages.Insert(0, new NetworkStatusMessage(NetworkStatusMessageType.Error, NewMessage));

			NewMessage = string.Empty;
		}

		private void ActivateExecute()
		{
			
		}

		private void SaveExecute()
		{
			DataItem.DisplayName = DisplayName;
			DataItem.PortNumber = PortNumber;
			DataItem.TransmitterType = TransmitterType;

			Title = DisplayName;

			_whenSaveRequested.OnNext(DataItem);
		}

		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield break;
		}

		private Subject<Transmitter> _whenSaveRequested = new Subject<Transmitter>();
		public IObservable<Transmitter> WhenSaveRequested => _whenSaveRequested;

		private TransmitterType _transmitterType;

		public TransmitterType TransmitterType
		{
			get { return _transmitterType; }
			set { SetValue(ref _transmitterType, value, nameof(TransmitterType)); }
		}

		private ObservableCollection<ViewModelBase> _messages;

		public ObservableCollection<ViewModelBase> Messages
		{
			get { return _messages ?? (_messages = new ObservableCollection<ViewModelBase>()); }
			set { SetValue(ref _messages, value, nameof(Messages)); }
		}

		private string _displayName;

		public string DisplayName
		{
			get { return _displayName; }
			set { SetValue(ref _displayName, value, nameof(DisplayName)); }
		}

		private int _portNumber;

		public int PortNumber
		{
			get { return _portNumber; }
			set { SetValue(ref _portNumber, value, nameof(PortNumber)); }
		}

		private string _newMessage;

		public string NewMessage
		{
			get { return _newMessage; }
			set { SetValue(ref _newMessage, value, nameof(NewMessage)); }
		}

		private ICommand _saveCommand;

		public ICommand SaveCommand
		{
			get { return _saveCommand; }
			set { SetValue(ref _saveCommand, value, nameof(SaveCommand)); }
		}

		private ICommand _clearLogCommand;

		public ICommand ClearLogCommand
		{
			get { return _clearLogCommand; }
			set { SetValue(ref _clearLogCommand, value, nameof(ClearLogCommand)); }
		}

		private ICommand _activateCommand;

		public ICommand ActivateCommand
		{
			get { return _activateCommand; }
			set { SetValue(ref _activateCommand, value, nameof(ActivateCommand)); }
		}

		private ICommand _newMessageCommand;

		public ICommand NewMessageCommand
		{
			get { return _newMessageCommand; }
			set { SetValue(ref _newMessageCommand, value, nameof(NewMessageCommand)); }
		}
	}
}