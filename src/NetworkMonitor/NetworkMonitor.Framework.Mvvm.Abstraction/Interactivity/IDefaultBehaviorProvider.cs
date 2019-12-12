using System.Collections.Generic;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IDefaultBehaviorProvider
	{
		IEnumerable<IBehavior> GetDefaultBehaviors();
	}
}