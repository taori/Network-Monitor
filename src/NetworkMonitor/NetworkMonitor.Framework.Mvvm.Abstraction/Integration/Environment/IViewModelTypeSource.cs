using System;
using System.Collections.Generic;
using NetworkMonitor.Framework.DependencyInjection;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment
{
	[InheritedMefExport(typeof(IViewModelTypeSource), LifeTime = LifeTime.Singleton)]
	public interface IViewModelTypeSource
	{
		IEnumerable<Type> GetValues();
	}
}