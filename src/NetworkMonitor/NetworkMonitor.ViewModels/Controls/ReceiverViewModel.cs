using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using NetworkMonitor.Framework.Mvvm.ViewModel;
using NetworkMonitor.Models.Entities;
using NetworkMonitor.Models.Enums;
using NetworkMonitor.ViewModels.Common;

namespace NetworkMonitor.ViewModels.Controls
{
	public class ReceiverViewModel : TabViewModel
	{
		public readonly Receiver DataItem;

		public ReceiverViewModel(Receiver dataItem)
		{
			Closable = true;
			DataItem = dataItem;
			DisplayName = dataItem.DisplayName;
			PortNumber = dataItem.PortNumber;
		}

		public ReceiverViewModel()
		{
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

		private ICommand _activateCommand;

		public ICommand ActivateCommand
		{
			get { return _activateCommand; }
			set { SetValue(ref _activateCommand, value, nameof(ActivateCommand)); }
		}

		private ReceiverType _receiverType;

		public ReceiverType ReceiverType
		{
			get { return _receiverType; }
			set { SetValue(ref _receiverType, value, nameof(ReceiverType)); }
		}

		protected override Task OnActivateAsync(IActivationContext context)
		{
			Title = DisplayName;
			SaveCommand = new ActionCommand(SaveExecute);
			ActivateCommand = new ActionCommand(ActivateExecute);
			return Task.CompletedTask;
		}

		private void SaveExecute()
		{
		}

		private void ActivateExecute()
		{
		}

		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield break;
		}
	}
}