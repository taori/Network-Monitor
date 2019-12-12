using System.Threading.Tasks;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IDeactivate
	{
		Task DeactivateAsync(IDeactivationContext context);

		event AsyncEventHandler OnDeactivated;
	}
}