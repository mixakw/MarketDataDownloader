#region FileHeader

// File:
// MarketDataDownloader/MarketDataDownloader.Logging/Logging.cs
// 
// Last updated:
// 2013-05-23 4:31 PM

#endregion

#region Usings

using System;
using Microsoft.Practices.Unity;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository.Hierarchy;

#endregion

namespace MarketDataDownloader.Logging
{
	public class MyLogger : IMyLogger
	{
		private readonly ILog _logger;

		public MyLogger(IAppender appender, string currentClassName)
		{
			InitializeHierarchyLogManager();
			BasicConfigurator.Configure(appender);
			_logger = LogManager.GetLogger(currentClassName);
		}

		public void Error(string message)
		{
			_logger.Error(message);
		}

		public void Error(string message, Exception exception)
		{
			_logger.Error(message, exception);
		}

		public void Info(string message)
		{
			_logger.Info(message);
		}

		public void Progress(string message)
		{
			_logger.Info(message);
		}

		public void Debug(string message)
		{
			_logger.Debug(message);
		}

		public void Warn(string message)
		{
			_logger.Warn(message);
		}

		private void InitializeHierarchyLogManager()
		{
			var hierarchy = (Hierarchy)LogManager.GetRepository();
			hierarchy.Root.RemoveAllAppenders();
		}
	}
}