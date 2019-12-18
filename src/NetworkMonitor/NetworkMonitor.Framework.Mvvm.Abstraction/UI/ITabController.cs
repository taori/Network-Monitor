namespace NetworkMonitor.Framework.Mvvm.Abstraction.UI
{
	public interface ITabController
	{
		bool IsOperational { get; }
		bool IsFocused(ITab model);
		void Insert(int index, ITab model);
		int Add(ITab model);
		void Remove(ITab model);
		int FindIndex(ITab model);
		void RemoveAt(int index);
		void Focus(ITab model);
		void FocusAt(int index);
		void FocusLast();
		int TabCount { get; }
	}
}