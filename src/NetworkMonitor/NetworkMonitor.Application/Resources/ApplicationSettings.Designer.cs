//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NetworkMonitor.Application.Resources
{


	[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.8.0.0")]
	internal sealed partial class ApplicationSettings : global::System.Configuration.ApplicationSettingsBase
	{

		private static ApplicationSettings defaultInstance = ((ApplicationSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new ApplicationSettings())));

		public static ApplicationSettings Default
		{
			get
			{
				return defaultInstance;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute("")]
		public string SettingsStorage
		{
			get
			{
				return ((string)(this["SettingsStorage"]));
			}
			set
			{
				this["SettingsStorage"] = value;
			}
		}
	}
}
