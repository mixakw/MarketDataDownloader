#region FileHeader

// File:
// MarketDataDownloader/MarketDataDownloader.Logger/MyLogger.cs
// 
// Last updated:
// 2013-05-23 10:45 AM

#endregion

#region Usings

using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

#endregion

namespace MarketDataDownloader.Logger
{
	public class MyLogger<T> : LogImpl, ILog<T>
	{
		private const string LogPattern = "%d{dd MMM HH:mm:ss} %-5p - %m%n";

		public MyLogger()
			: base(LogManager.GetLogger(typeof (T).Name).Logger)
		{
			var hierarchy = (Hierarchy) LogManager.GetRepository();
			var tracer = new TraceAppender();

			var patternLayout = new PatternLayout();
			patternLayout.ConversionPattern = LogPattern;
			patternLayout.ActivateOptions();

			tracer.Layout = patternLayout;
			tracer.ActivateOptions();
			hierarchy.Root.AddAppender(tracer);

			var richTextBoxAppender = new RichTextBoxAppender();
			richTextBoxAppender.Layout = patternLayout;
			richTextBoxAppender.ActivateOptions();

			hierarchy.Root.AddAppender(richTextBoxAppender);
			hierarchy.Root.Level = Level.All;
			hierarchy.Configured = true;
		}

		public void AddAppender(IAppender appender)
		{
			var hierarchy =
				(Hierarchy) LogManager.GetRepository();

			hierarchy.Root.AddAppender(appender);
		}
	}
}