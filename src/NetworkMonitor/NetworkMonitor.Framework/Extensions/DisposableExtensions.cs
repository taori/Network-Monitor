using System;
using System.Reactive.Disposables;

namespace NetworkMonitor.Framework.Extensions
{
	public static class DisposableExtensions
	{
		public static void DisposeWith(this IDisposable source, CompositeDisposable with)
		{
			with.Add(source);
		}
	}
}