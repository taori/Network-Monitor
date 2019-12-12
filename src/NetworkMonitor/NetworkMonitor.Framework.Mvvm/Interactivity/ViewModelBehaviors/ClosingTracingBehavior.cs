using System.Threading.Tasks;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NLog;

namespace NetworkMonitor.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public class ClosingTracingBehavior : AsyncBehaviorBase<IWindowClosingBehaviorContext>
	{
		/// <inheritdoc />
		public ClosingTracingBehavior(string message)
		{
			Message = message;
		}

		private static readonly ILogger Log = LogManager.GetLogger(nameof(ClosingTracingBehavior));

		public string Message { get; set; }

		/// <inheritdoc />
		protected override Task OnExecuteAsync(IWindowClosingBehaviorContext context)
		{
			if (!string.IsNullOrEmpty(Message))
				Log.Info(Message);
			return Task.CompletedTask;
		}
	}
}