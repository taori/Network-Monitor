using AutoMapper;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;
using NetworkMonitor.ViewModels.Services;
namespace NetworkMonitor.Application.Dependencies.Configuration
{
	public class ApplicationSettings : IApplicationSettings
	{
		private const string StorageKey = "LocalApplicationSettings";
		private readonly ISettingsStorage _storage;

		private static readonly ApplicationSettings Default = new ApplicationSettings();

		public ApplicationSettings() { }

		public ApplicationSettings(ISettingsStorage storage)
		{
			_storage = storage;

			if (!_storage.TryGetValue<ApplicationSettings>(StorageKey, out var oldSettings))
			{
				oldSettings = Default;
			}

			CreateMapper().Map(oldSettings, this);
		}

		private static Mapper CreateMapper()
		{
			return new Mapper(new MapperConfiguration(config =>
			{
				config.CreateMap<ApplicationSettings, ApplicationSettings>();
			}));
		}

		public bool FocusTabOnCreate { get; set; } = true;
		public bool FocusTabOnOpen { get; set; } = true;

		public void Update()
		{
			_storage.UpdateValue(StorageKey, this);
			_storage.Save();
		}
	}
}