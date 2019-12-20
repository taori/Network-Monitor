using System;
using System.Net;
using System.Text;
using NetworkMonitor.Models.Enums;

namespace NetworkMonitor.Models.Entities
{

	public class Receiver
	{
		public Guid Id { get; set; }

		public string DisplayName { get; set; }

		public int PortNumber { get; set; }

		public ReceiverType ReceiverType { get; set; }

		public string IpAddress { get; set; }

		public bool Broadcast { get; set; }

		public Encoding Encoding { get; set; }

		public bool IsOperational()
		{
			if (PortNumber <= 0)
				return false;

			if (!Broadcast && !IPAddress.TryParse(IpAddress, out _))
				return false;

			return true;
		}
	}
}
