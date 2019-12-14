using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;

namespace NetworkMonitor.Framework.Mvvm.ViewModel
{
	public abstract class TabViewModel : ContentViewModel, ITab
	{
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

		public virtual Task<bool> TryCloseTabAsync()
		{
			return Task.FromResult(true);
		}
	}
}