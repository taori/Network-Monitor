namespace NetworkMonitor.Framework.Mvvm.Abstraction.UI
{
	public interface ITabController
	{
		void InsertAt(int index, object model);
		void Insert(object model);
		void Remove(object model);
		void RemoveAt(int index);
		void Focus(object model);
		void FocusAt(int index);
		int TabCount { get; }
	}
}