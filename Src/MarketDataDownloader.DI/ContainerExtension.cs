using IQFeed.Core;
using MarketDataDownloader.DomainLogicLayer.Abstraction;
using MarketDataDownloader.DomainLogicLayer.Logger;
using Microsoft.Practices.Unity;
using log4net;

namespace MarketDataDownloader.DI
{
	public class ContainerExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Container.RegisterType<ILog>(new InjectionFactory(x => LogManager.GetLogger(typeof(IDataFeedCore))));
			Container.RegisterType<ILog>(new InjectionFactory(x => LogManager.GetLogger(typeof(IDataFeedProxy))));
			Container.RegisterType<ILog>(new InjectionFactory(x => LogManager.GetLogger(typeof(IDataFeedQueryBuilder))));
			Container.RegisterType<ILog>(new InjectionFactory(x => LogManager.GetLogger(typeof(IDataFeedSaver))));

			Container.RegisterType<LoggerResolver>(new InjectionFactory(x => LogManager.GetLogger(typeof(IDataFeedSaver))));

			Container.RegisterType<IDataFeedCore, IQFeedCore>(new ContainerControlledLifetimeManager());
			Container.RegisterType<IDataFeedProxy, IQFeedProxy>(new ContainerControlledLifetimeManager());
			Container.RegisterType<IDataFeedQueryBuilder, IQFeedQueryBuilder>(new ContainerControlledLifetimeManager());
			Container.RegisterType<IDataFeedSaver, IQFeedDataSaver>(new ContainerControlledLifetimeManager());
		}
	}
}
