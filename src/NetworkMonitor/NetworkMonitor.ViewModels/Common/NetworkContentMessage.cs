using System;
using NetworkMonitor.Framework.Mvvm.ViewModel;

namespace NetworkMonitor.ViewModels.Common
{
	public class NetworkContentMessage : ViewModelBase
	{
		private DateTime _dateTime;

		public DateTime DateTime
		{
			get { return _dateTime; }
			set { SetValue(ref _dateTime, value, nameof(DateTime)); }
		}

		private string _message;

		public string Message
		{
			get { return _message; }
			set { SetValue(ref _message, value, nameof(Message)); }
		}

		public NetworkContentMessage(string message)
		{
			_message = message;
			_dateTime = DateTime.Now;
		}
	}
}