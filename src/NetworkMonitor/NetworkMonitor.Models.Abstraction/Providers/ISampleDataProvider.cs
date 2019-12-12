
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkMonitor.Framework.DataAccess;
using NetworkMonitor.Framework.DependencyInjection;
using NetworkMonitor.Models.Abstraction.Entities;

namespace NetworkMonitor.Models.Abstraction.Providers
{
	[InheritedMefExport(typeof(ISampleDataProvider))]
	public interface ISampleDataProvider : IDataProvider
	{
		Task<IEnumerable<ISampleData>> GetAllAsync(int count);
	}
}