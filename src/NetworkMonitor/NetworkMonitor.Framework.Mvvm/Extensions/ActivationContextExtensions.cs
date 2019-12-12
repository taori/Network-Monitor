using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Navigation;
using NetworkMonitor.Framework.Mvvm.Abstraction.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace NetworkMonitor.Framework.Mvvm.Extensions
{
	public static class ActivationContextExtensions
	{
		public static Task<bool> NavigateAsync(this IActivationContext context, IWindowViewModel viewModel, string windowId)
		{
			return context.ServiceProvider.GetService<INavigationService>().OpenWindowAsync(viewModel, windowId);
		}
	}
}