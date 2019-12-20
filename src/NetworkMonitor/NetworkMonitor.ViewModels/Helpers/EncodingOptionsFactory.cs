using System.Collections.Generic;
using System.Text;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;

namespace NetworkMonitor.ViewModels.Helpers
{
	public class EncodingOptionsFactory
	{
		public static IEnumerable<SelectorOption<Encoding>> GetAll()
		{
			yield return new SelectorOption<Encoding>(Encoding.UTF8, Encoding.UTF8.BodyName);
			yield return new SelectorOption<Encoding>(Encoding.ASCII, Encoding.ASCII.BodyName);
			yield return new SelectorOption<Encoding>(Encoding.BigEndianUnicode, Encoding.BigEndianUnicode.BodyName);
			yield return new SelectorOption<Encoding>(Encoding.UTF32, Encoding.UTF32.BodyName);
			yield return new SelectorOption<Encoding>(Encoding.UTF7, Encoding.UTF7.BodyName);
			yield return new SelectorOption<Encoding>(Encoding.Unicode, Encoding.Unicode.BodyName);
		}
	}
}