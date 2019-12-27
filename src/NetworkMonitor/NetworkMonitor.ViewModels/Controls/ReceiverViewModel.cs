using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using NetworkMonitor.Framework;
using NetworkMonitor.Framework.Logging;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using NetworkMonitor.Framework.Mvvm.Commands;
using NetworkMonitor.Framework.Mvvm.ViewModel;
using NetworkMonitor.Models.Entities;
using NetworkMonitor.Models.Enums;
using NetworkMonitor.ViewModels.Common;
using NetworkMonitor.ViewModels.Helpers;
using NetworkMonitor.ViewModels.Services;

namespace NetworkMonitor.ViewModels.Controls
{
	public class ReceiverViewModel : TabViewModel
	{
		public readonly Receiver DataItem;

		public ReceiverViewModel(Receiver dataItem)
		{
			Closable = true;
			DataItem = dataItem;
			DisplayName = DataItem.DisplayName;
			PortNumber = DataItem.PortNumber;
			ReceiverType = DataItem.ReceiverType;
			IpAddress = DataItem.IpAddress;
			Broadcast = DataItem.Broadcast;
			Encoding = DataItem.Encoding;

			SaveCommand = new TaskCommand(SaveExecute, d => !IsActive && CanSave);
			ToggleCommand = new TaskCommand(ToggleExecute, d => CanToggle);
			ClearMessagesCommand = new TaskCommand(ClearMessagesExecute, d => Messages.Count > 0);
			Encodings = new ObservableCollection<SelectorOption<Encoding>>(EncodingOptionsFactory.GetAll());

			CanToggle = true;

			WhenPropertyChanged.Subscribe(name =>
			{
				CanSave = true;
				CanToggle = false;

				if (name == nameof(IsActive))
					OnPropertyChanged(nameof(ToggleMessage));
			});
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

		private int _portNumber;

		public int PortNumber
		{
			get { return _portNumber; }
			set { SetValue(ref _portNumber, value, nameof(PortNumber)); }
		}

		private string _displayName;

		public string DisplayName
		{
			get { return _displayName; }
			set { SetValue(ref _displayName, value, nameof(DisplayName)); }
		}

		private ICommand _saveCommand;

		public ICommand SaveCommand
		{
			get { return _saveCommand; }
			set { SetValue(ref _saveCommand, value, nameof(SaveCommand)); }
		}

		private ICommand _toggleCommand;

		public ICommand ToggleCommand
		{
			get { return _toggleCommand; }
			set { SetValue(ref _toggleCommand, value, nameof(ToggleCommand)); }
		}

		private ICommand _clearMessagesCommand;

		public ICommand ClearMessagesCommand
		{
			get { return _clearMessagesCommand; }
			set { SetValue(ref _clearMessagesCommand, value, nameof(ClearMessagesCommand)); }
		}

		private bool _isActive;

		public bool IsActive
		{
			get { return _isActive; }
			set { SetValue(ref _isActive, value, nameof(IsActive)); }
		}

		public string ToggleMessage
		{
			get { return _isActive ? "Deactivate" : "Activate"; }
			set { }
		}

		private ReceiverType _receiverType;

		public ReceiverType ReceiverType
		{
			get { return _receiverType; }
			set { SetValue(ref _receiverType, value, nameof(ReceiverType)); }
		}

		private ObservableCollection<ViewModelBase> _messages;

		public ObservableCollection<ViewModelBase> Messages
		{
			get { return _messages ?? (_messages = new ObservableCollection<ViewModelBase>()); }
			set { SetValue(ref _messages, value, nameof(Messages)); }
		}

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

		private Subject<Receiver> _whenSaveRequested = new Subject<Receiver>();
		public IObservable<Receiver> WhenSaveRequested => _whenSaveRequested;

		protected override Task OnActivateAsync(IActivationContext context)
		{
			Title = DisplayName;
			
			return Task.CompletedTask;
		}

		private Task ClearMessagesExecute(object parameter)
		{
			Messages.Clear();
			return Task.CompletedTask;
		}

		private Task SaveExecute(object parameter)
		{
			Title = DisplayName;

			DataItem.PortNumber = PortNumber;
			DataItem.ReceiverType = ReceiverType;
			DataItem.DisplayName = DisplayName;
			DataItem.IpAddress = IpAddress;
			DataItem.Broadcast = Broadcast;
			DataItem.Encoding = Encoding;

			_whenSaveRequested.OnNext(DataItem);

			CanToggle = true;

			return Task.CompletedTask;
		}

		private IReceiverClient CreateClient(Receiver receiver)
		{
			switch (DataItem.ReceiverType)
			{
				case ReceiverType.Tcp:
					return new TcpReceiverClient(receiver, new DelegateLogger(DisplayInformation, DisplayWarning, DisplayError, DisplayFatal));
				case ReceiverType.Udp:
					return new UdpReceiverClient(receiver, new DelegateLogger(DisplayInformation, DisplayWarning, DisplayError, DisplayFatal));
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
			AddContentMessage(obj);
		}

		private IReceiverClient _receiverClient;
		private Task ToggleExecute(object parameter)
		{
			if (_receiverClient == null || !IsActive)
			{
				if (_receiverClient != null)
					AddStatusMessage(NetworkStatusMessageType.Connection, "Terminating old client.");

				_receiverClient?.Terminate();
				AddStatusMessage(NetworkStatusMessageType.Connection, "Creating new client.");
				_receiverClient = CreateClient(DataItem);
				_receiverClient.WhenReceived
					.ObserveOn(Application.Current.Dispatcher)
					.Subscribe(ClientReceivedMessage);

				AddStatusMessage(NetworkStatusMessageType.Connection, "Starting new client.");
				if (_receiverClient.Execute())
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
				if (_receiverClient != null)
					AddStatusMessage(NetworkStatusMessageType.Connection, "Terminating old client.");

				_receiverClient?.Terminate();

				IsActive = false;
			}

			return Task.CompletedTask;
		}

		private void ClientReceivedMessage(NetworkContent obj)
		{
			var encoding = DataItem.Encoding ?? Encoding.UTF8;
			AddContentMessage($"Received content ({encoding.BodyName}): \"{obj.Content}\" from \"{obj.Source}\".");
		}

		private void AddStatusMessage(NetworkStatusMessageType statusType, string message)
		{
			Messages.Insert(0, new NetworkStatusMessage(statusType, message));
		}

		private void AddContentMessage(string message)
		{
			Messages.Insert(0, new NetworkContentMessage(message));
		}

		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield break;
		}
	}
}