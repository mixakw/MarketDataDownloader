using Microsoft.Practices.Unity;
using log4net;

namespace MarketDataDownloader.DI
{
	public class ContainerExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Container.RegisterType<ILog>(new InjectionFactory(x => LogManager.GetLogger(typeof(IQFeedProxy))));
			Container.RegisterType<IQFeedProxy>();

			//this.Container.RegisterType<BasketRepository, SqlBasketRepository>(new PerResolveLifetimeManager(), sqlCtorParam);
			//this.Container.RegisterType<DiscountRepository, SqlDiscountRepository>(new PerResolveLifetimeManager(), sqlCtorParam);
			//this.Container.RegisterType<BasketDiscountPolicy, RepositoryBasketDiscountPolicy>(new PerResolveLifetimeManager());
			//this.Container.RegisterType<IBasketService, BasketService>(new PerResolveLifetimeManager());
			//this.Container.RegisterType<CurrencyProvider, SqlCurrencyProvider>(new PerResolveLifetimeManager(), sqlCtorParam);
		}
	}
}
