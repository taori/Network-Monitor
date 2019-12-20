using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Subjects;
using JetBrains.Annotations;

namespace NetworkMonitor.Framework.Mvvm.ViewModel
{
	public abstract class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private Subject<string> _whenPropertyChanged = new Subject<string>();
		public IObservable<string> WhenPropertyChanged => _whenPropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public bool SetValue<T>(ref T field, T value, string propertyName)
		{
			if (EqualityComparer<T>.Default.Equals(field, value))
				return false;

			PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			_whenPropertyChanged.OnNext(propertyName);
			return true;
		}

		public event PropertyChangingEventHandler PropertyChanging;
	}
}
