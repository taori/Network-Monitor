using System;
using System.Collections.Generic;
using NetworkMonitor.Framework.DependencyInjection;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping
{
	[InheritedMefExport(typeof(IDataTemplateMapper), LifeTime = LifeTime.Singleton)]
	public interface IDataTemplateMapper
	{
		IEnumerable<(Type viewModelType, Type viewType)> GetMappings(IEnumerable<Type> viewModelTypes, IEnumerable<Type> viewTypes);
	}
}