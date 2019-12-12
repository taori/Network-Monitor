using System;
using System.Windows;
using NetworkMonitor.Application.Views.Windows;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer;
using NetworkMonitor.Framework.Mvvm.ViewModel;

namespace NetworkMonitor.Application.Dependencies.UI
{
	public class WindowFactory : IViewModelWindowFactory
	{
		/// <inheritdoc />
		public bool CanCreateWindow(object dataContext)
		{
			if (dataContext == null)
			{
				throw new ArgumentNullException(nameof(dataContext), nameof(dataContext));
			}

			return true;
		}

		/// <inheritdoc />
		public Window CreateWindow(object dataContext)
		{
			if (dataContext is DefaultWindowViewModel)
				return new DefaultWindow();

			return new DefaultWindow();
		}
	}
}