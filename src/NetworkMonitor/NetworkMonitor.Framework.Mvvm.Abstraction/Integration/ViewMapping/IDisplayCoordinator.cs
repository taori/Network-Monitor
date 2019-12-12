using System.Threading.Tasks;
using NetworkMonitor.Framework.DependencyInjection;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping
{
	[InheritedMefExport(typeof(IDisplayCoordinator), LifeTime = LifeTime.PerRequest)]
	public interface IDisplayCoordinator
	{
		/// <summary>
		/// Order in which the VisualizerFactory will be attempt to construct a Visualizer
		/// </summary>
		int Priority { get; }

		bool CanProcess(object dataContext);

		Task<bool> DisplayAsync(object dataContext, ICoordinationArguments coordinationArguments);
	}
}