using System;

namespace NetworkMonitor.Framework.DependencyInjection
{
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = true)]
	public class PartCreationPolicyAttribute : Attribute
	{
		internal const string DefaultNoShare = "___None";
		internal const string DefaultShared = "___Shared";

		public string SharingBoundary { get; }

		public PartCreationPolicyAttribute(string sharingBoundary)
		{
			SharingBoundary = sharingBoundary;
		}

		public PartCreationPolicyAttribute(bool shared)
		{
			if (shared)
			{
				SharingBoundary = DefaultShared;
			}
			else
			{
				SharingBoundary = DefaultNoShare;
			}
		}
	}
}