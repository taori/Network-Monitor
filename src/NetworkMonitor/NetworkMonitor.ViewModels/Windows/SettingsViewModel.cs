﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Behaviors.Core;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.ViewModel;
using NetworkMonitor.ViewModels.Services;

namespace NetworkMonitor.ViewModels.Windows
{
	public class SettingsViewModel: WindowContentViewModelBase
	{
		protected override Task OnActivateAsync(IActivationContext context)
		{
			SaveCommand = new ActionCommand(SaveExecute);
			var settings = ServiceProvider.GetRequiredService<IApplicationSettings>();
			FocusTabOnCreate = settings.FocusTabOnCreate;
			FocusTabOnOpen = settings.FocusTabOnOpen;
			return Task.CompletedTask;
		}

		private void SaveExecute(object obj)
		{
			var settings = ServiceProvider.GetRequiredService<IApplicationSettings>();
			settings.FocusTabOnCreate = FocusTabOnCreate;
			settings.FocusTabOnOpen = FocusTabOnOpen;
			settings.Update();

			this.Window?.Close();
		}

		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield return new RestoreWindowDimensionsBehavior();
		}

		public override string GetTitle()
		{
			return "Application settings";
		}

		private ICommand _saveCommand;

		public ICommand SaveCommand
		{
			get { return _saveCommand; }
			set { SetValue(ref _saveCommand, value, nameof(SaveCommand)); }
		}

		private bool _focusTabOnCreate;

		public bool FocusTabOnCreate
		{
			get { return _focusTabOnCreate; }
			set { SetValue(ref _focusTabOnCreate, value, nameof(FocusTabOnCreate)); }
		}

		private bool _focusTabOnOpen;

		public bool FocusTabOnOpen
		{
			get { return _focusTabOnOpen; }
			set { SetValue(ref _focusTabOnOpen, value, nameof(FocusTabOnOpen)); }
		}
	}
}