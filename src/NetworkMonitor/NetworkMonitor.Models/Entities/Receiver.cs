using System;
using NetworkMonitor.Models.Enums;

namespace NetworkMonitor.Models.Entities
{

	public class Receiver
	{
		public Guid Id { get; set; }

		public string DisplayName { get; set; }

		public int PortNumber { get; set; }

		public ReceiverType ReceiverType { get; set; }
	}
}
