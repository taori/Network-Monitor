using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkMonitor.Models.Entities;

namespace NetworkMonitor.Models.Providers
{
	public interface ITransmitterProvider
	{
		Task<List<Transmitter>> GetAllAsync();

		Task SaveAsync(Transmitter item);
		Task DeleteAsync(Transmitter item);
		Task<Transmitter> CopyAsync(Transmitter item);
	}
}