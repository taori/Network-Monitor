using System;
using System.Threading.Tasks;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.UI
{
	public interface ITab
	{
		string Title { get; }
		IObservable<string> WhenTitleChanged { get; }
		bool Closable { get; }
		IObservable<bool> WhenClosableChanged { get; }
		Task<bool> TryCloseTabAsync();
	}
}