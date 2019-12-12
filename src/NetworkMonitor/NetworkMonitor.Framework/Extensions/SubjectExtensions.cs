using System.Reactive.Subjects;

namespace NetworkMonitor.Framework.Extensions
{
	public static class SubjectExtensions
	{
		public static bool TryOnNext<T>(this Subject<T> source, T value)
		{
			if (source.IsDisposed)
				return false;
			source.OnNext(value);
			return true;
		}
	}
}