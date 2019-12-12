using System;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.UI
{
	public class SelectorOption<TValue> where TValue : struct, Enum
	{
		public TValue Value { get; set; }

		public string Text { get; set; }

		public static implicit operator SelectorOption<TValue>((string text, TValue value) source)
		{
			return new SelectorOption<TValue>(){Text = source.text, Value = source.value};
		}

		public static implicit operator SelectorOption<TValue>((TValue value, string text) source)
		{
			return new SelectorOption<TValue>(){Text = source.text, Value = source.value};
		}
	}
}