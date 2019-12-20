namespace NetworkMonitor.ViewModels.Services
{
	public interface IApplicationSettings
	{
		bool FocusTabOnCreate { get; set; }
		bool FocusTabOnOpen { get; set; }
		void Update();
	}
}