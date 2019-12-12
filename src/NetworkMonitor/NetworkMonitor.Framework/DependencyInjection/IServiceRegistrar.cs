using Microsoft.Extensions.DependencyInjection;

namespace NetworkMonitor.Framework.DependencyInjection
{
	public interface IServiceRegistrar
	{
		void Register(IServiceCollection services);
	}
}