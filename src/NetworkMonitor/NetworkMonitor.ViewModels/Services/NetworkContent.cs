using System.Net;

namespace NetworkMonitor.ViewModels.Services
{
	public class NetworkContent
	{
		public NetworkContent(string content, IPEndPoint source)
		{
			Content = content;
			Source = source;
		}

		public string Content { get; set; }

		public IPEndPoint Source { get; set; }
	}
}