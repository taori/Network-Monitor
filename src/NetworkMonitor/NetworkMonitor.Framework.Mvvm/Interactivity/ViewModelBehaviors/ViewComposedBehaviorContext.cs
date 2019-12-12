using System;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Composer;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using JetBrains.Annotations;

namespace NetworkMonitor.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public class ViewComposedBehaviorContext : IViewComposedBehaviorContext
	{
		public IViewCompositionContext CompositionContext { get; }
		public IServiceContext ServiceContext { get; }

		public ViewComposedBehaviorContext([NotNull] IViewCompositionContext compositionContext, [NotNull] IServiceContext serviceContext)
		{
			CompositionContext = compositionContext ?? throw new ArgumentNullException(nameof(compositionContext));
			ServiceContext = serviceContext ?? throw new ArgumentNullException(nameof(serviceContext));
		}
	}
}