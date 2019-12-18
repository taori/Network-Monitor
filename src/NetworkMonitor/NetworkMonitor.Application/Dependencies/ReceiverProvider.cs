using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;
using NetworkMonitor.Models.Entities;
using NetworkMonitor.Models.Providers;

namespace NetworkMonitor.Application.Dependencies
{
	public class ReceiverProvider : IReceiverProvider
	{
		private readonly ISettingsStorage _settingsStorage;
		private const string StorageKey = "ReceiverProvider";

		public ReceiverProvider(ISettingsStorage settingsStorage)
		{
			_settingsStorage = settingsStorage;
		}

		public Task<List<Receiver>> GetAllAsync()
		{
			if(!_settingsStorage.TryGetValue(StorageKey, out List<Receiver> receivers))
				receivers = new List<Receiver>();

			return Task.FromResult(receivers);
		}

		public async Task SaveAsync(Receiver item)
		{
			var receivers = await GetAllAsync();
			var index = receivers.FindIndex(d => d.Id == item.Id);
			if (index >= 0)
			{
				receivers.RemoveAt(index);
			}

			receivers.Add(item);

			_settingsStorage.UpdateValue(StorageKey, receivers);
			_settingsStorage.Save();
		}

		public async Task DeleteAsync(Receiver item)
		{
			var receivers = await GetAllAsync();
			var index = receivers.FindIndex(d => d.Id == item.Id);
			if (index >= 0)
			{
				receivers.RemoveAt(index);
			}

			_settingsStorage.UpdateValue(StorageKey, receivers);
			_settingsStorage.Save();
		}
	}
}