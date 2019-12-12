using System.Threading.Tasks;
using System.Windows;

namespace NetworkMonitor.Application.Views.Windows
{
	/// <summary>
	/// Interaktionslogik für Splash.xaml
	/// </summary>
	public partial class Splash : Window
	{
		public Splash()
		{
			Loaded += OnLoaded;
			InitializeComponent();
		}

		private async void OnLoaded(object sender, RoutedEventArgs e)
		{
			Loaded -= OnLoaded;
			await Task.Delay(5000);
			Close();
		}
	}
}
