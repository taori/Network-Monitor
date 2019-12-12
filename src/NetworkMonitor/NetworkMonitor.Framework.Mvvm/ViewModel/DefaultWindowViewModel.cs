using NetworkMonitor.Framework.Mvvm.Abstraction.ViewModel;
using JetBrains.Annotations;

namespace NetworkMonitor.Framework.Mvvm.ViewModel
{
	public class DefaultWindowViewModel : WindowViewModelBase
	{
		/// <inheritdoc />
		public DefaultWindowViewModel([NotNull] IWindowContentViewModel content) : base(content)
		{
		}
	}
}