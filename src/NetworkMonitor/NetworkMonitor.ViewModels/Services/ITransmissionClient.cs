using System.Text;
using System.Threading.Tasks;

namespace NetworkMonitor.ViewModels.Services
{
	public interface ITransmissionClient
	{
		bool Execute();
		void Terminate();
		Task<int> SendAsync(byte[] bytes);
	}
}