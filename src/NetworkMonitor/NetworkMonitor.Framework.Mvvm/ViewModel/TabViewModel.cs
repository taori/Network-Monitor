using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;

namespace NetworkMonitor.Framework.Mvvm.ViewModel
{
	public abstract class TabViewModel : ContentViewModel, ITab
	{
		protected TabViewModel()
		{
			TabCloseCommand = new ActionCommand(TabCloseExecute);
		}

		private void TabCloseExecute(object obj)
		{
			_whenCloseRequested.OnNext(null);
		}

		private string _title;

		public string Title
		{
			get { return _title; }
			set
			{
				if (SetValue(ref _title, value, nameof(Title)))
				{
					_whenTitleChanged.OnNext(value);
				}
			}
		}

		private ICommand _tabCloseCommand;

		public ICommand TabCloseCommand
		{
			get { return _tabCloseCommand; }
			set { SetValue(ref _tabCloseCommand, value, nameof(TabCloseCommand)); }
		}

		private Subject<string> _whenTitleChanged = new Subject<string>();
		public IObservable<string> WhenTitleChanged => _whenTitleChanged;

		private bool _closable;

		public bool Closable
		{
			get { return _closable; }
			set
			{
				if (SetValue(ref _closable, value, nameof(Closable)))
				{
					_whenClosableChanged.OnNext(value);
				}
			}
		}

		private Subject<bool> _whenClosableChanged = new Subject<bool>();
		public IObservable<bool> WhenClosableChanged => _whenClosableChanged;

		private Subject<object> _whenCloseRequested = new Subject<object>();
		public IObservable<object> WhenCloseRequested => _whenCloseRequested;
		public void CloseTab()
		{
			TabCloseExecute(null);
		}
	}
}