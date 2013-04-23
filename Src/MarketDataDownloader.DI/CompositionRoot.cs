using Microsoft.Practices.Unity;

namespace MarketDataDownloader.DI
{
	public class CompositionRoot
	{
		public CompositionRoot()
		{
			var container = new UnityContainer();

			container.AddNewExtension<ContainerExtension>();
		}
	}
}
