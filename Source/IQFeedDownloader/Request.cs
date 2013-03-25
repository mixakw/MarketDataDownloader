using System;
using System.Collections.Generic;

namespace IQFeedDownloader
{
	public class Request
	{
		#region Public Variables

		public string TickDaysHeader = "HTD";
		public string TickIntervalHeader = "HTT";
		public string IntradayDaysHeader = "HID";
		public string IntradayIntervalHeader = "HIT";
		public string DailyDaysHeader = "HDX";
		public string DailyIntervalHeader = "HDT";
		public string WeeklyDaysHeader = "HWX";
		public string MonthlyDaysHeader = "HMX";

		public string Delimiter = ",";
		public string TerminatingCharacter = Environment.NewLine;

		#endregion Public Variables

		#region Private Variables

		private List<string> _symbols;
		private string _currentSymbol;
		private string _days;
		private string _maxDatapoints = "0";
		private string _dataDirection = "1";
		private string _requestID;
		private string _datapointsPerSend = "2500";
		private int _interval;

		private string _beginFilterTime = "000000";
		private string _endFilterTime = "235959";

		private string _beginDate;
		private string _endDate;
		private string _beginTime = "000000";
		private string _endTime = "000000";

		private string _timeFrameName;
		private string _timeFrameType;

		#endregion Private Variables

		#region Constructor

		public Request()
		{
			_symbols = new List<string>();
		}

		#endregion Constructor

		#region Public Properties

		public List<string> Symbols
		{
			get { return _symbols; }
			set { _symbols = value; }
		}

		public string CurrentSymbol
		{
			get { return _currentSymbol; }
			set { _currentSymbol = value; }
		}

		public string Days
		{
			get { return _days; }
			set { _days = value; }
		}

		public string MaxDatapoints
		{
			get { return _maxDatapoints; }
			set { _maxDatapoints = value; }
		}

		public string BeginFilterTime
		{
			get { return _beginFilterTime; }
			set { _beginFilterTime = value; }
		}

		public string EndFilterTime
		{
			get { return _endFilterTime; }
			set { _endFilterTime = value; }
		}

		public string DataDirection
		{
			get { return _dataDirection; }
			set { _dataDirection = value; }
		}

		public string RequestID
		{
			get { return _requestID; }
			set { _requestID = value; }
		}

		public string DatapointsPerSend
		{
			get { return _datapointsPerSend; }
			set { _datapointsPerSend = value; }
		}

		public string BeginTime
		{
			get { return _beginTime; }
			set { _beginTime = value; }
		}

		public string EndTime
		{
			get { return _endTime; }
			set { _endTime = value; }
		}

		public string BeginDate
		{
			get { return _beginDate; }
			set { _beginDate = value; }
		}

		public string EndDate
		{
			get { return _endDate; }
			set { _endDate = value; }
		}

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

		#endregion Public Properties
	}
}
