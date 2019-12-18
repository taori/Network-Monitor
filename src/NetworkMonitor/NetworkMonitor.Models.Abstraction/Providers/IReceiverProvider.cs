using System.Collections.Generic;

namespace NetworkMonitor.Models.Abstraction.Providers
{
	public interface ITransmitterProvider
	{

	}
	public interface IReceiverProvider
	{
		IEnumerable<Receiver>
	}
}