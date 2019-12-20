using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkMonitor.Models.Entities;

namespace NetworkMonitor.Models.Providers
{
	public interface IReceiverProvider
	{
		Task<List<Receiver>> GetAllAsync();

		Task SaveAsync(Receiver item);
		Task DeleteAsync(Receiver item);
		Task<Receiver> CopyAsync(Receiver item);
	}
}