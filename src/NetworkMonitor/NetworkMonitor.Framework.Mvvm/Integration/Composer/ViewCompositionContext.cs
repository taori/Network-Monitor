using System;
using System.Windows;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using JetBrains.Annotations;

namespace NetworkMonitor.Framework.Mvvm.Integration.Composer
{
	public class ViewCompositionContext : IViewCompositionContext
	{
		/// <inheritdoc />
		public ViewCompositionContext([NotNull] FrameworkElement control, [NotNull] object dataContext, [NotNull] ICoordinationArguments coordinationArguments)
		{
			Control = control ?? throw new ArgumentNullException(nameof(control));
			DataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
			CoordinationArguments = coordinationArguments ?? throw new ArgumentNullException(nameof(coordinationArguments));
		}

		/// <inheritdoc />
		public FrameworkElement Control { get; }

		/// <inheritdoc />
		public object DataContext { get; }

		/// <inheritdoc />
		public ICoordinationArguments CoordinationArguments { get; }
	}
}