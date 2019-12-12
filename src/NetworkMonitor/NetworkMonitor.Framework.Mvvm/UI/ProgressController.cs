using System;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;

namespace NetworkMonitor.Framework.Mvvm.UI
{
	public class ProgressController : IDisposable
	{
		public IProgressController ProgressAdapter { get; }

		public ProgressController(IProgressController progressAdapter)
		{
			ProgressAdapter = progressAdapter;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			ProgressAdapter?.Dispose();
		}
	}
}