using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using NetworkMonitor.Framework.Logging;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using NetworkMonitor.Framework.Mvvm.Commands;
using NetworkMonitor.Framework.Mvvm.ViewModel;
using NetworkMonitor.Models.Entities;
using NetworkMonitor.ViewModels.Common;
using NetworkMonitor.ViewModels.Helpers;
using NetworkMonitor.ViewModels.Services;
using NetworkMonitor.ViewModels.Windows;
using TransmitterType = NetworkMonitor.Models.Enums.TransmitterType;

namespace NetworkMonitor.ViewModels.Controls
{
	public class TransmitterViewModel : TabViewModel
	{
		public readonly Transmitter DataItem;
		private readonly IDialogService _dialogService;
		private readonly MainViewModel _mainViewModel;

		public TransmitterViewModel(Transmitter dataItem, IDialogService dialogService, MainViewModel mainViewModel)
		{
			DataItem = dataItem;
			_dialogService = dialogService;
			_mainViewModel = mainViewModel;

			Closable = true;
			DisplayName = DataItem.DisplayName;
			PortNumber = DataItem.PortNumber;
			TransmitterType = DataItem.TransmitterType;
			Broadcast = DataItem.Broadcast;
			IpAddress = DataItem.IpAddress;
			Encoding = DataItem.Encoding;

			SaveCommand = new TaskCommand(SaveExecute, d => !IsActive && CanSave);
			ToggleCommand = new TaskCommand(ToggleExecute, d => CanToggle);
			NewMessageCommand = new TaskCommand(NewMessageExecute);
			ClearLogCommand = new TaskCommand(ClearLogExecute, d => Messages.Count > 0);
			Encodings = new ObservableCollection<SelectorOption<Encoding>>(EncodingOptionsFactory.GetAll());

			CanToggle = true;
			
			WhenPropertyChanged.Subscribe(name =>
			{
				if (!new[] { nameof(CanSave), nameof(CanToggle), nameof(Title), nameof(IsActive), nameof(Messages) }.Contains(name))
				{
					CanSave = true;
					CanToggle = false;
				}

				if(name == nameof(IsActive))
					OnPropertyChanged(nameof(ToggleMessage));
			});
		}

		protected override Task OnActivateAsync(IActivationContext context)
		{
			Title = DisplayName;
			return Task.CompletedTask;
		}

		private Task ClearLogExecute(object parameter)
		{
			Messages.Clear();
			return Task.CompletedTask;
		}

		private async Task NewMessageExecute()
		{
			if (!IsActive)
			{
				if (!await _dialogService.YesNoAsync(_mainViewModel,
					"The client is not running yet. Do you want to start it?"))
					return;

				await ToggleExecute(null);
			}

			var encoding = DataItem.Encoding ?? Encoding.UTF8;
			var bytes = encoding.GetBytes(NewMessage);

			var bytesSent = await _transmissionClient.SendAsync(bytes);
			if (bytesSent == bytes.Length)
			{
				AddContentMessage($"Message \"{NewMessage}\" using encoding {encoding.BodyName} sent.");
			}
			else
			{
				AddStatusMessage( NetworkStatusMessageType.Error, $"Sending message \"{NewMessage}\" using encoding {encoding.BodyName} failed.");
			}
				
			NewMessage = string.Empty;
		}

		private ITransmissionClient CreateClient(Transmitter transmitter)
		{
			switch (DataItem.TransmitterType)
			{
				case TransmitterType.Tcp:
					return new TcpTransmissionClient(transmitter, new DelegateLogger(DisplayInformation, DisplayWarning, DisplayError, DisplayFatal));
				case TransmitterType.Udp:
					return new UdpTransmissionClient(transmitter, new DelegateLogger(DisplayInformation, DisplayWarning, DisplayError, DisplayFatal));
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void DisplayFatal(string obj)
		{
			AddStatusMessage(NetworkStatusMessageType.Error, obj);
		}

		private void DisplayError(string obj)
		{
			AddStatusMessage(NetworkStatusMessageType.Error, obj);
		}

		private void DisplayWarning(string obj)
		{
			AddStatusMessage(NetworkStatusMessageType.Information, obj);
		}

		private void DisplayInformation(string obj)
		{
			AddStatusMessage(NetworkStatusMessageType.Information, obj);
		}

		private ITransmissionClient _transmissionClient;
		private Task ToggleExecute(object parameter)
		{
			if (_transmissionClient == null || !IsActive)
			{
				if (_transmissionClient != null)
					AddStatusMessage(NetworkStatusMessageType.Connection, "Terminating old client.");

				_transmissionClient?.Terminate();
				AddStatusMessage(NetworkStatusMessageType.Connection, "Creating new client.");
				_transmissionClient = CreateClient(DataItem);
				AddStatusMessage(NetworkStatusMessageType.Connection, "Starting new client.");
				if (_transmissionClient.Execute())
				{
					AddStatusMessage(NetworkStatusMessageType.Connection, "Client running.");
					IsActive = true;
				}
				else
				{
					AddStatusMessage(NetworkStatusMessageType.Error, "Client failed to start.");
				}
			}
			else
			{
				if (_transmissionClient != null)
					AddStatusMessage(NetworkStatusMessageType.Connection, "Terminating old client.");

				_transmissionClient?.Terminate();

				IsActive = false;
			}

			return Task.CompletedTask;
		}

		private void AddStatusMessage(NetworkStatusMessageType statusType, string message)
		{
			Messages.Insert(0, new NetworkStatusMessage(statusType, message));
		}

		private void AddContentMessage(string message)
		{
			Messages.Insert(0, new NetworkContentMessage(message));
		}

		private Task SaveExecute(object parameter)
		{
			DataItem.DisplayName = DisplayName;
			DataItem.PortNumber = PortNumber;
			DataItem.TransmitterType = TransmitterType;
			DataItem.Broadcast = Broadcast;
			DataItem.IpAddress = IpAddress;
			DataItem.Encoding = Encoding;

			Title = DisplayName;

			_whenSaveRequested.OnNext(DataItem);

			CanToggle = true;
			CanSave = false;

			return Task.CompletedTask;
		}

		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield break;
		}

		private bool _canToggle;

		public bool CanToggle
		{
			get => _canToggle;
			set => SetValue(ref _canToggle, value, nameof(CanToggle));
		}

		private bool _canSave;

		public bool CanSave
		{
			get => _canSave;
			set => SetValue(ref _canSave, value, nameof(CanSave));
		}

		public string ToggleMessage
		{
			get { return _isActive ? "Deactivate" : "Activate"; }
			set { }
		}

		private Encoding _encoding;

		public Encoding Encoding
		{
			get { return _encoding; }
			set { SetValue(ref _encoding, value, nameof(Encoding)); }
		}

		private ObservableCollection<SelectorOption<Encoding>> _encodings;

		public ObservableCollection<SelectorOption<Encoding>> Encodings
		{
			get { return _encodings ?? (_encodings = new ObservableCollection<SelectorOption<Encoding>>()); }
			set { SetValue(ref _encodings, value, nameof(Encodings)); }
		}

		private Subject<Transmitter> _whenSaveRequested = new Subject<Transmitter>();
		public IObservable<Transmitter> WhenSaveRequested => _whenSaveRequested;

		private bool _broadcast;

		public bool Broadcast
		{
			get { return _broadcast; }
			set { SetValue(ref _broadcast, value, nameof(Broadcast)); }
		}

		private string _ipAddress;

		public string IpAddress
		{
			get { return _ipAddress; }
			set { SetValue(ref _ipAddress, value, nameof(IpAddress)); }
		}

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

		private bool _isActive;

		public bool IsActive
		{
			get { return _isActive; }
			set { SetValue(ref _isActive, value, nameof(IsActive)); }
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

		private ICommand _toggleCommand;

		public ICommand ToggleCommand
		{
			get { return _toggleCommand; }
			set { SetValue(ref _toggleCommand, value, nameof(ToggleCommand)); }
		}

		private ICommand _newMessageCommand;

		public ICommand NewMessageCommand
		{
			get { return _newMessageCommand; }
			set { SetValue(ref _newMessageCommand, value, nameof(NewMessageCommand)); }
		}
	}
}