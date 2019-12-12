using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;

namespace NetworkMonitor.Framework.Mvvm.Integration.ViewMapping
{
	public class WindowArguments : ICoordinationArguments
	{
		/// <inheritdoc />
		public WindowArguments(string windowId)
		{
			WindowId = windowId;
		}

		public string WindowId { get; }
	}
}