using System.Net;

namespace NetworkMonitor.ViewModels.Services
{
	public class NetworkContent
	{
		public NetworkContent(string content, EndPoint source)
		{
			Content = content;
			Source = source.ToString();
		}
		public NetworkContent(string content, IPEndPoint source)
		{
			Content = content;
			Source = source.ToString();
		}

		public string Content { get; }
		
		public string Source { get; }
	}
}