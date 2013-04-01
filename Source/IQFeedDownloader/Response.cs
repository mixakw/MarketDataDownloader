using System;

namespace IQFeedDownloader
{
	internal sealed class Response
	{
		private Parameters _parameters;

		public string RequestId { get; set; }
		public string High { get; set; }
		public string Low { get; set; }
		public string Open { get; set; }
		public string Close { get; set; }
		public string Volume { get; set; }
		public string OpenInterest { get; set; }
		public string Last { get; set; }
		public string LastSize { get; set; }
		public string TotalVolume { get; set; }
		public string Bid { get; set; }
		public string Ask { get; set; }
		public string TickId { get; set; }
		public string BidSize { get; set; }
		public string AskSize { get; set; }
		public string TradeType { get; set; }

		public string ResponseDate { get; set; }
		public string ResponseTime { get; set; }
		public string ResponseDateTime { get; set; }

		public Response()
		{
			_parameters = new Parameters();
		}

		public void ParseTickMarketData(string[] input)
		{
			if (input == null) throw new ArgumentNullException("input");
			if (input.Length != 10) throw new ArgumentException("input");

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
			if (input.Length != 7) throw new ArgumentException("input");

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

			string date = ParseDate(input[1]);
			string time = ParseTime(input[1]);

			ResponseDateTime = date + _parameters.DateTimeDelimeter + time;
		}

		private string ParseTime(string input)
		{
			string time = input.Substring(11, 5);
			return time.ToString();//_parameters.TimeFormat);
		}

		private string ParseDate(string input)
		{
			string date = input.Substring(0, 4) + input.Substring(5, 2) + input.Substring(8, 2);
			return date.ToString();//_parameters.DateFormat);
		}
	}
}
