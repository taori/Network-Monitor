using NetworkMonitor.Framework.DependencyInjection;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment
{
	[InheritedMefExport(typeof(IConfigurationRunner), LifeTime = LifeTime.Singleton)]
	public interface IConfigurationRunner
	{
		void Execute();
	}
}