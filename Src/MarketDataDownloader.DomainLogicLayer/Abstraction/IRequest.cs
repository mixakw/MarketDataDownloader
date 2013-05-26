// =================================================
// File:
// MarketDataDownloader/MarketDataDownloader.DomainLogicLayer/IRequest.cs
// 
// Last updated:
// 2013-05-24 4:40 PM
// =================================================

#region Usings

using System.Collections.Generic;

#endregion

namespace MarketDataDownloader.DomainLogicLayer.Abstraction
{
	public interface IRequest
	{
		List <string> Symbols { get; set; }
		string CurrentSymbol { get; set; }
		string Days { get; set; }
		string MaxDatapoints { get; set; }
		string BeginFilterTime { get; set; }
		string EndFilterTime { get; set; }
		string DataDirection { get; set; }
		string RequestID { get; set; }
		string DatapointsPerSend { get; set; }
		string BeginTime { get; set; }
		string EndTime { get; set; }
		string BeginDate { get; set; }
		string EndDate { get; set; }
		string TimeFrameName { set; }
		string TimeFrame { get; }
		string TimeFrameType { set; }
		string BeginDateTime { get; }
		string EndDateTime { get; }
		int Interval { get; set; }
	}
}
