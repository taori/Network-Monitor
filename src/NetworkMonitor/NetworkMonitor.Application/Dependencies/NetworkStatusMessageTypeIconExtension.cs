using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using MahApps.Metro.IconPacks;
using NetworkMonitor.ViewModels.Common;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace NetworkMonitor.Application.Dependencies
{
	public class NetworkStatusMessageTypeIconExtension : BasePackIconExtension, IValueConverter
	{
		private static readonly NetworkStatusMessageTypeIconExtension Instance =
			new NetworkStatusMessageTypeIconExtension();

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return Instance;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return CreateImageSource(value);
		}

		private static ImageSource CreateImageSource(object value)
		{
			if (value is NetworkStatusMessageType type)
			{
				switch (type)
				{
					case NetworkStatusMessageType.Connection:
						return CreateCustomImageSource(PackIconMaterialKind.AccessPoint, Brushes.Yellow);
					case NetworkStatusMessageType.Information:
						return CreateCustomImageSource(PackIconMaterialKind.InformationOutline, Brushes.Black);
					case NetworkStatusMessageType.Error:
						return CreateCustomImageSource(PackIconMaterialKind.Alert, Brushes.Red);
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			return CreateCustomImageSource(PackIconMaterialKind.InformationOutline, Brushes.Black);
		}

		private static ImageSource CreateCustomImageSource(PackIconMaterialKind kind, Brush brush)
		{
			var extension = new MaterialImageExtension(kind);
			extension.Brush = brush;
			return extension.ProvideValue(null) as ImageSource;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}