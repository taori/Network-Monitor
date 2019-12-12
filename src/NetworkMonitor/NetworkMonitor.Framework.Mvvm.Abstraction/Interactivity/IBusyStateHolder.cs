namespace NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IBusyStateHolder
	{
		IBusyState LoadingState { get; }
	}
}