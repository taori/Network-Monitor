using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.ViewModel;
using NetworkMonitor.Models.Entities;
using NetworkMonitor.Models.Enums;

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
		}

		public TransmitterViewModel()
		{
			
		}

		protected override Task OnActivateAsync(IActivationContext context)
		{
			Title = DisplayName;
			SaveCommand = new ActionCommand(SaveExecute);
			ActivateCommand = new ActionCommand(ActivateExecute);
			return Task.CompletedTask;
		}

		private void ActivateExecute()
		{
			
		}

		private void SaveExecute()
		{
		}

		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield break;
		}

		private TransmitterType _transmitterType;

		public TransmitterType TransmitterType
		{
			get { return _transmitterType; }
			set { SetValue(ref _transmitterType, value, nameof(TransmitterType)); }
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
	}
}