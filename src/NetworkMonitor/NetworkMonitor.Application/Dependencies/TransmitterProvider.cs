using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;
using NetworkMonitor.Models.Entities;
using NetworkMonitor.Models.Providers;

namespace NetworkMonitor.Application.Dependencies
{
	public class TransmitterProvider : ITransmitterProvider
	{
		private readonly ISettingsStorage _settingsStorage;
		private const string StorageKey = "TransmitterProvider";

		public TransmitterProvider(ISettingsStorage settingsStorage)
		{
			_settingsStorage = settingsStorage;
		}

		public Task<List<Transmitter>> GetAllAsync()
		{
			if (!_settingsStorage.TryGetValue(StorageKey, out List<Transmitter> result))
				result = new List<Transmitter>();

			return Task.FromResult(result);
		}

		public async Task SaveAsync(Transmitter item)
		{
			var allItems = await GetAllAsync();
			var index = allItems.FindIndex(d => d.Id == item.Id);
			if (index >= 0)
			{
				allItems.RemoveAt(index);
			}

			allItems.Add(item);

			_settingsStorage.UpdateValue(StorageKey, allItems);
			_settingsStorage.Save();
		}

		public async Task DeleteAsync(Transmitter item)
		{
			var allItems = await GetAllAsync();
			var index = allItems.FindIndex(d => d.Id == item.Id);
			if (index >= 0)
			{
				allItems.RemoveAt(index);
			}

			_settingsStorage.UpdateValue(StorageKey, allItems);
			_settingsStorage.Save();
		}

		public async Task<Transmitter> CopyAsync(Transmitter item)
		{
			var all = await GetAllAsync();
			var index = all.FindIndex(d => d.Id == item.Id);
			if (index < 0)
			{
				throw new Exception($"item with id {item.Id} not found.");
			}

			var clone = all[index].Clone() as Transmitter;
			clone.Id = Guid.NewGuid();
			all.Add(clone);

			_settingsStorage.UpdateValue(StorageKey, all);
			_settingsStorage.Save();

			return clone;
		}
	}
}