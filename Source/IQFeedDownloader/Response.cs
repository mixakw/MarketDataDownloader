using System;

namespace IQFeedDownloader
{
	internal sealed class Response
	{
		#region Private Variables

		private string _requestId;

		private string _high;
		private string _low;
		private string _open;
		private string _close;
		private string _volume;

		private string _openInterest;
		private string _last;
		private string _lastSize;
		private string _totalVolume;
		private string _bid;
		private string _ask;
		private string _tickId;
		private string _bidSize;
		private string _askSize;
		private string _tradeType;

		private string _formattedDate;
		private string _formattedTime;

		private string _dateFormat;
		private string _timeFormat;
		private string _dateTimeDelimeter;

		#endregion Private Variables

		#region Public Properties

		public string RequestId
		{
			get { return _requestId; }
			set { _requestId = value; }
		}

		public string High
		{
			get { return _high; }
			set { _high = value; }
		}

		public string Low
		{
			get { return _low; }
			set { _low = value; }
		}

		public string Open
		{
			get { return _open; }
			set { _open = value; }
		}

		public string Close
		{
			get { return _close; }
			set { _close = value; }
		}

		public string Volume
		{
			get { return _volume; }
			set { _volume = value; }
		}

		public string OpenInterest
		{
			get { return _openInterest; }
			set { _openInterest = value; }
		}

		public string Last
		{
			get { return _last; }
			set { _last = value; }
		}

		public string LastSize
		{
			get { return _lastSize; }
			set { _lastSize = value; }
		}

		public string TotalVolume
		{
			get { return _totalVolume; }
			set { _totalVolume = value; }
		}

		public string Bid
		{
			get { return _bid; }
			set { _bid = value; }
		}

		public string Ask
		{
			get { return _ask; }
			set { _ask = value; }
		}

		public string TickId
		{
			get { return _tickId; }
			set { _tickId = value; }
		}

		public string BidSize
		{
			get { return _bidSize; }
			set { _bidSize = value; }
		}

		public string AskSize
		{
			get { return _askSize; }
			set { _askSize = value; }
		}

		public string TradeType
		{
			get { return _tradeType; }
			set { _tradeType = value; }
		}

		public string FormattedDate
		{
			get { return _formattedDate; }
			set { _formattedDate = value; }
		}

		public string FormattedTime
		{
			get { return _formattedTime; }
			set { _formattedTime = value; }
		}

		public string DateFormat
		{
			get { return _dateFormat; }
			set { _dateFormat = value; }
		}

		public string TimeFormat
		{
			get { return _timeFormat; }
			set { _timeFormat = value; }
		}

		public string DateTimeDelimeter
		{
			get { return _dateTimeDelimeter; }
			set { _dateTimeDelimeter = value; }
		}

		#endregion Public Properties

		#region Public Methods

		public void ParseTickMarketData(string[] input)
		{
			if (input == null) throw new ArgumentNullException("input");

			RequestId = input[0];
			Last = input[2];
			LastSize = input[3];
			TotalVolume = input[4];
			Bid = input[5];
			Ask = input[6];
			TickId = input[7];
			BidSize = input[8];
			AskSize = input[9];
			TradeType = input[10];
		}

		public void ParseMarketData(string[] input)
		{
			if (input == null) throw new ArgumentNullException("input");

			RequestId = input[0];
			High = input[2];
			Low = input[3];
			Open = input[4];
			Close = input[5];
			OpenInterest = input[6];
			Volume = input[7];
		}

		public void ParseDateTime(string[] input)
		{
			if (input == null) throw new ArgumentNullException("input");

			FormattedDate = ParseDate(input[1]);
			FormattedTime = ParseTime(input[1]);
		}

		public string FormattedDateTime()
		{
			return FormattedDate + DateTimeDelimeter + FormattedTime;
		}

		#endregion Public Methods

		#region Private Methods

		private string ParseTime(string input)
		{
			var result = DateTime.Parse(input.Substring(11, 5));
			return result.ToString(TimeFormat);
		}

		private string ParseDate(string input)
		{
			var yearMonthDay = DateTime.Parse(input.Substring(0, 4) + input.Substring(5, 2) + input.Substring(8, 2));
			return yearMonthDay.ToString(DateFormat);
		}

		#endregion Private Methods
	}
}
