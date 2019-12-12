using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.UI
{
	public interface IDialogService
	{
		Task DisplayMessageAsync(object owner, string title, string message);
		Task<bool> YesNoAsync(object owner, string message, string title = null);
		Task<bool?> YesNoCancelAsync(object owner, string message, string title = null);
		Task<string> GetTextAsync(object owner, string title, string message);
		Task<IProgressController> ShowProgressAsync(object owner, string title, string message, bool cancelable, Action<IProgressController> abortion);
	}
}