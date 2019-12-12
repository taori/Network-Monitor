using System;
using System.Collections.Generic;
using NetworkMonitor.Framework.DependencyInjection;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment
{
	[InheritedMefExport(typeof(IViewTypeSource), LifeTime = LifeTime.Singleton)]
	public interface IViewTypeSource
	{
		IEnumerable<Type> GetValues();
	}
}