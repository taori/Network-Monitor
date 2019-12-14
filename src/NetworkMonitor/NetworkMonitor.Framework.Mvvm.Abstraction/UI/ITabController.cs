namespace NetworkMonitor.Framework.Mvvm.Abstraction.UI
{
	public interface ITabController
	{
		bool IsOperational { get; }
		void Insert(int index, ITab model);
		void Add(ITab model);
		void Remove(ITab model);
		int FindIndex(ITab model);
		void RemoveAt(int index);
		void Focus(ITab model);
		void FocusAt(int index);
		void FocusLast();
		int TabCount { get; }
	}
}