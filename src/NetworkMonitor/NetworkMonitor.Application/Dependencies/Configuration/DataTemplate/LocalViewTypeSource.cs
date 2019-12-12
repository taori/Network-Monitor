using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMonitor.Application.Views.Windows;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;

namespace NetworkMonitor.Application.Dependencies.Configuration.DataTemplate
{
	public class LocalViewTypeSource : IViewTypeSource
	{
		/// <inheritdoc />
		public IEnumerable<Type> GetValues()
		{
			return typeof(DefaultWindow).Assembly.ExportedTypes.Where(d => d.FullName.StartsWith("NetworkMonitor.Application.Views"));
		}
	}
}