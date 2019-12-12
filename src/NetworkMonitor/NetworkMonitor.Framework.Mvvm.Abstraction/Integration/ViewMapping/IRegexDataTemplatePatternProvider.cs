using System.Text.RegularExpressions;

namespace NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping
{
	public interface IRegexDataTemplatePatternProvider
	{
		Regex ViewModelTypeRegex { get; }
		Regex ViewTypeRegex { get; }
	}
}