using System.Collections.Generic;
using System.Reflection;
using NetworkMonitor.Framework.Controls;
using NetworkMonitor.Framework.DataAccess;
using NetworkMonitor.Framework.DependencyInjection;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using NetworkMonitor.Framework.Mvvm.Integration.ViewMapping;
using NetworkMonitor.Models.Providers;
using NetworkMonitor.Shared.Utility;

namespace NetworkMonitor.Application.Dependencies.Setup
{
	public class InjectionAssemblyLoader : IInjectionAssemblyLoader
	{
		/// <inheritdoc />
		public IEnumerable<Assembly> GetAssemblies()
		{
			yield return typeof(IDataProvider).Assembly;
			yield return typeof(IReceiverProvider).Assembly;
			yield return typeof(DependencyContainer).Assembly;
			yield return typeof(IRegionManager).Assembly;
			yield return typeof(RegionManager).Assembly;
			yield return typeof(FileHelper).Assembly;
			yield return typeof(OverlayPanel).Assembly;
		}
	}
}