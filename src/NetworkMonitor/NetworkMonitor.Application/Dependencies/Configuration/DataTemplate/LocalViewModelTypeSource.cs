using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;

namespace NetworkMonitor.Application.Dependencies.Configuration.DataTemplate
{
	public class LocalViewModelTypeSource : IViewModelTypeSource
	{
		/// <inheritdoc />
		public IEnumerable<Type> GetValues()
		{
			return typeof(ViewModels.Common.RegionNames).Assembly.ExportedTypes.Where(d => d.FullName.StartsWith("NetworkMonitor.ViewModels"));
		}
	}
}