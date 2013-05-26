using System;
using MarketDataDownloader.UI;
using log4net;
using log4net.Config;

namespace MarketDataDownloader.DomainLogicLayer.Logger
{
	public class LoggerResolver
	{
		public ILog Logger { get; private set; }

		public LoggerResolver(Type type)
		{
			Logger = LogManager.GetLogger(type);
			XmlConfigurator.Configure();
			new RichTextBoxAppender(MainForm.Rt);
		}
	}
}
