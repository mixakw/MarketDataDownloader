using System;
using System.Collections.Generic;

namespace IQFeedDownloader
{
	public class Request
	{
		public string Delimiter = ",";
		public string Terminater = Environment.NewLine;

		public string TickDaysHeader = "HTD";
		public string TickIntervalHeader = "HTT";
		public string IntradayDaysHeader = "HID";
		public string IntradayIntervalHeader = "HIT";
		public string DailyDaysHeader = "HDX";
		public string DailyIntervalHeader = "HDT";
		public string WeeklyDaysHeader = "HWX";
		public string MonthlyDaysHeader = "HMX";

		private int _interval;
		private string _timeFrameName;
		private string _timeFrameType;

		public Request()
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

		public string TimeFrame
		{
			get { return _timeFrameName + " " + _timeFrameType; }
		}

		public string TimeFrameName
		{
			set { _timeFrameName = value; }
		}

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

		public int Interval
		{
			get { return _interval * 60; }
			set { _interval = value; }
		}
	}
}
