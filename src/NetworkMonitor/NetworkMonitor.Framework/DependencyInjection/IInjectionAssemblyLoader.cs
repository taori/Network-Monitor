using System.Collections.Generic;
using System.Reflection;

namespace NetworkMonitor.Framework.DependencyInjection
{
	public interface IInjectionAssemblyLoader
	{
		IEnumerable<Assembly> GetAssemblies();
	}
}