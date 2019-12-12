using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;

namespace NetworkMonitor.Framework.Mvvm.Integration.ViewMapping
{
	public class RegionArguments : ICoordinationArguments
	{
		/// <inheritdoc />
		public RegionArguments(object regionManagerReference, string targetRegion)
		{
			RegionManagerReference = regionManagerReference;
			TargetRegion = targetRegion;
		}

		public object RegionManagerReference { get; set; }

		public string TargetRegion { get; set; }
	}
}