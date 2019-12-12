using System.Threading.Tasks;
using System.Windows;
using NetworkMonitor.Framework.DependencyInjection;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer
{
	[InheritedMefExport(typeof(IViewComposer))]
	public interface IViewComposer
	{
		/// <summary>
		/// Order in which the Composer is prioritized for a specific dataContext
		/// </summary>
		int Priority { get; }

		Task<bool> ComposeAsync(IViewCompositionContext context);

		bool CanHandle(FrameworkElement control);
	}
}