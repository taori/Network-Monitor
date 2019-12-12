using System;
using System.Windows.Media;
using System.Threading.Tasks;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.UI
{
	public interface IProgressController : IDisposable
	{
		void SetIndeterminate();
		void SetCancelable(bool value);
		void SetProgress(double value);
		void SetMessage(string message);
		void SetTitle(string title);
		void SetProgressBarForegroundBrush(Brush brush);
		Task CloseAsync();
		bool IsCanceled { get; }
		bool IsOpen { get; }
		double Minimum { get; set; }
		double Maximum { get; set; }
		IObservable<EventArgs> WhenClosed { get; }
		IObservable<EventArgs> WhenCanceled { get; }
	}
}