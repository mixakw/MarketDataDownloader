#region FileHeader

// File:
// MarketDataDownloader/MarketDataDownloader.Logging/ILogging.cs
// 
// Last updated:
// 2013-05-23 4:16 PM

#endregion

#region Usings

using System;

#endregion

namespace MarketDataDownloader.Logging
{
	public interface IMyLogger
	{
		void Error(string message);
		void Error(string message, Exception exception);
		void Progress(string message);
		void Info(string message);
		void Debug(string message);
		void Warn(string message);
	}
}