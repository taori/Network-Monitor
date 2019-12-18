using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace NetworkMonitor.Framework.Controls
{
	[ContentProperty(nameof(ContentControl.Content))]
	public class ContentPanel : ContentControl
	{
		static ContentPanel()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentPanel),
				new FrameworkPropertyMetadata(typeof(ContentPanel)));
		}
	}
}