using System.Collections.Generic;
using MarketDataDownloader.DomainLogicLayer.Abstraction;

namespace IQFeed.Models
{
	public class IQFeedRequest : IRequest
	{
		public List<string> Symbols { get; set; }

		public string CurrentSymbol { get; set; }
		public string Days { get; set; }
		public string MaxDatapoints { get; set; }
		public string BeginFilterTime { get; set; }
		public string EndFilterTime { get; set; }
		public string DataDirection { get; set; }
		public string RequestID { get; set; }
		public string DatapointsPerSend { get; set; }
		public string BeginTime { get; set; }
		public string EndTime { get; set; }
		public string BeginDate { get; set; }
		public string EndDate { get; set; }

		private string _timeFrameName;
		public string TimeFrameName
		{
			set { _timeFrameName = value; }
		}

		public string TimeFrame
		{
			get { return _timeFrameName + " " + _timeFrameType; }
		}

		private string _timeFrameType;
		public string TimeFrameType
		{
			set { _timeFrameType = value; }
		}

		public string BeginDateTime
		{
			get { return BeginDate + " " + BeginTime; }
		}

		public string EndDateTime
		{
			get { return EndDate + " " + EndTime; }
		}

		private int _interval;
		

		public int Interval
		{
			get { return _interval * 60; }
			set { _interval = value; }
		}

		public IQFeedRequest()
		{
			MaxDatapoints = "0";
			BeginFilterTime = "000000";
			EndFilterTime = "235959";
			DataDirection = "1";
			DatapointsPerSend = "2500";
			BeginTime = "000000";
			EndTime = "000000";
			Symbols = new List<string>();
		}
	}
}
