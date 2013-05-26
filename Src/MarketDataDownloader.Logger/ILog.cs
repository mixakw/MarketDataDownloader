#region FileHeader

// File:
// MarketDataDownloader/MarketDataDownloader.Logger/ILog.cs
// 
// Last updated:
// 2013-05-23 10:45 AM

#endregion

#region Usings

using log4net;

#endregion

namespace MarketDataDownloader.Logger
{
	public interface ILog<T> : ILog
	{
	}
}