using System;
using System.Collections.Generic;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IBehaviorHost : IDisposable
	{
		List<IBehavior> Behaviors { get; }
	}
}