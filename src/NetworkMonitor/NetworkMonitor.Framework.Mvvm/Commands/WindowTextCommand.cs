using System.Windows.Input;
using System.Windows.Media;
using NetworkMonitor.Framework.Mvvm.Abstraction.ViewModel;
using NetworkMonitor.Framework.Mvvm.ViewModel;

namespace NetworkMonitor.Framework.Mvvm.Commands
{
	public class WindowTextCommand : ViewModelBase, IWindowCommand
	{
		public WindowTextCommand(ICommand command, string text)
		{
			Command = command;
			Text = text;
		}

		private ICommand _command;

		public ICommand Command
		{
			get { return _command; }
			set { SetValue(ref _command, value, nameof(Command)); }
		}

		private string _text;

		public string Text
		{
			get { return _text; }
			set { SetValue(ref _text, value, nameof(Text)); }
		}
	}

	public class WindowImageCommand : IWindowCommand
	{
		public WindowImageCommand(ICommand command, ImageSource imageSource)
		{
			Command = command;
			ImageSource = imageSource;
		}

		public ICommand Command { get; set; }

		public ImageSource ImageSource { get; set; }
	}
}