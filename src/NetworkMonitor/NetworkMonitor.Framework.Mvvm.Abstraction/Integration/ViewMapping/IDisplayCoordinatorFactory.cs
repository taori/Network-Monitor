namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping
{
	public interface IDisplayCoordinatorFactory
	{
		IDisplayCoordinator Create(object dataContext);
	}
}