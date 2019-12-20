using System.Globalization;
using System.Net;
using System.Windows.Controls;

namespace NetworkMonitor.Application.Dependencies
{
	public class IpValidator : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (value == null)
				return ValidationResult.ValidResult;

			if (value is string s)
			{
				if(string.IsNullOrEmpty(s))
					return ValidationResult.ValidResult;

				if (IPAddress.TryParse(s, out _))
					return ValidationResult.ValidResult;

				return new ValidationResult(false, "Failed to validate Ip Address. ip4/ip6 expected.");
			}

			return new ValidationResult(false, "Input should be string");
		}
	}
}