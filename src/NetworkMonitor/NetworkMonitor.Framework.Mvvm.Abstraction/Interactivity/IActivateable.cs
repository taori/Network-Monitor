using System;
using System.Threading.Tasks;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IActivateable : IDisposable
	{
		Task ActivateAsync(IActivationContext context);

		IObservable<IActivationContext> WhenActivated { get; }
	}
}