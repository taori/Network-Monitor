using System;
using System.Net;
using System.Text;
using NetworkMonitor.Models.Enums;
using Newtonsoft.Json;

namespace NetworkMonitor.Models.Entities
{
	public class Transmitter : ICloneable
	{
		public Guid Id { get; set; }

		public string DisplayName { get; set; }

		public int PortNumber { get; set; }

		public bool Broadcast { get; set; }

		public TransmitterType TransmitterType { get; set; }

		public string IpAddress { get; set; }

		[JsonIgnore]
		public Encoding Encoding
		{
			get => Encoding.GetEncoding(EncodingName ?? Encoding.UTF8.WebName);
			set => EncodingName = value.WebName;
		}

		public string EncodingName { get; set; }

		public bool IsOperational()
		{
			if (PortNumber <= 0)
				return false;

			if (!Broadcast && !IPAddress.TryParse(IpAddress, out _))
				return false;

			return true;
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}