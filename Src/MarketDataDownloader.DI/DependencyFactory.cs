// =================================================
// File:
// MarketDataDownloader/MarketDataDownloader.DI/DependencyFactory.cs
// 
// Last updated:
// 2013-05-24 3:56 PM
// =================================================

#region Usings

using IQFeed.Core;
using IQFeed.Helpers;

using MarketDataDownloader.DomainLogicLayer.Abstraction;
using MarketDataDownloader.DomainLogicLayer.Models;
using MarketDataDownloader.Logging;

using Microsoft.Practices.Unity;

using log4net.Appender;

#endregion

namespace MarketDataDownloader.DI
{
	/// <summary> Simple wrapper for unity resolution. </summary>
	public class DependencyFactory
	{
		/// <summary>
		///     Static constructor for DependencyFactory which will
		///     initialize the unity container.
		/// </summary>
		static DependencyFactory()
		{
			Container = new UnityContainer();

			Container.RegisterInstance(new IQFeedResponseHelper(), new ContainerControlledLifetimeManager());
			Container.RegisterInstance(new Parameters(), new ContainerControlledLifetimeManager());

			Container.RegisterType <IAppender, RichTextBoxAppender>(new ContainerControlledLifetimeManager());
			Container.RegisterType <IMyLogger, MyLogger>(new ContainerControlledLifetimeManager());
			Container.RegisterType <IDataFeedCore, IQFeedCore>(new ContainerControlledLifetimeManager());
			Container.RegisterType <IDataFeedProxy, IQFeedProxy>(new ContainerControlledLifetimeManager());
			Container.RegisterType <IDataFeedQueryBuilder, IQFeedQueryBuilder>(new ContainerControlledLifetimeManager());
			Container.RegisterType <IDataFeedSaver, IQFeedDataSaver>(new ContainerControlledLifetimeManager());
		}

		/// <summary>
		///     Public reference to the unity container which will
		///     allow the ability to register instances or take other
		///     actions on the container.
		/// </summary>
		public static IUnityContainer Container { get; private set; }
	}
}
