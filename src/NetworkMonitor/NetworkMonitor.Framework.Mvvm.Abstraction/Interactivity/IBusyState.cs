using System;
using System.ComponentModel;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IBusyState : INotifyPropertyChanged
	{
		bool IsBusy { get; set; }
		IObservable<PropertyChangedEventArgs> WhenPropertyChanged { get; }
		void Increment();
		void Decrement();
		IDisposable Session();
	}
}