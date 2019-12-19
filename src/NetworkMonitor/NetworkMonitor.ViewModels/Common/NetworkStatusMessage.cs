using System;
using NetworkMonitor.Framework.Mvvm.ViewModel;

namespace NetworkMonitor.ViewModels.Common
{
	public class NetworkStatusMessage : ViewModelBase
	{
		public NetworkStatusMessage(NetworkStatusMessageType type, string message)
		{
			_type = type;
			_message = message;
			_dateTime = DateTime.Now;
		}

		private NetworkStatusMessageType _type;

		public NetworkStatusMessageType Type
		{
			get { return _type; }
			set { SetValue(ref _type, value, nameof(Type)); }
		}

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
	}
}