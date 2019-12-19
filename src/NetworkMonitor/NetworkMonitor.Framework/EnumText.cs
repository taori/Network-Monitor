using System;

namespace NetworkMonitor.Framework
{
	public class EnumText<T> where T : struct, Enum
	{
		public string Text { get; set; }

		public T Value { get; set; }
	}
}