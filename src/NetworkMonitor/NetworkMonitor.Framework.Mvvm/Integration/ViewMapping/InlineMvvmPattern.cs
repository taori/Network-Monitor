using System.Text.RegularExpressions;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;

namespace NetworkMonitor.Framework.Mvvm.Integration.ViewMapping
{
	public class InlineMvvmPattern : IRegexDataTemplatePatternProvider
	{
		/// <inheritdoc />
		public InlineMvvmPattern(Regex viewModelTypeRegex, Regex viewTypeRegex)
		{
			ViewModelTypeRegex = viewModelTypeRegex;
			ViewTypeRegex = viewTypeRegex;
		}

		/// <inheritdoc />
		public Regex ViewModelTypeRegex { get; }

		/// <inheritdoc />
		public Regex ViewTypeRegex { get; }
	}
}