using System;
using System.Globalization;
using IQFeed.Models;

namespace IQFeed.Helpers
{
	public class IQFeedResponseHelper
	{
		private readonly IQFeedResponse _response;

		public IQFeedResponseHelper(IQFeedResponse response)
		{
			if (response == null) throw new ArgumentNullException("response");

			_response = response;
		}

		public IQFeedResponse ParseTickMarketData(string[] input)
		{
			if (input == null) throw new ArgumentNullException("input");
			if (input.Length != 10) throw new ArgumentException("input.Length");

			_response.RequestId = input[0];
			_response.Last = input[2];
			_response.LastSize = input[3];
			_response.TotalVolume = input[4];
			_response.Bid = input[5];
			_response.Ask = input[6];
			_response.TickId = input[7];
			_response.BidSize = input[8];
			_response.AskSize = input[9];
			_response.TradeType = input[10];

			return _response;
		}

		public IQFeedResponse ParseMarketData(string[] input)
		{
			if (input == null) throw new ArgumentNullException("input");
			if (input.Length != 7) throw new ArgumentException("input.Length");

			_response.RequestId = input[0];
			_response.High = input[2];
			_response.Low = input[3];
			_response.Open = input[4];
			_response.Close = input[5];
			_response.OpenInterest = input[6];
			_response.Volume = input[7];

			return _response;
		}

		public string ParseDateTime(string[] input, string delimiter, string timeFormat, string dateFormat)
		{
			if (input == null) throw new ArgumentNullException("input");
			if (string.IsNullOrEmpty(delimiter)) throw new ArgumentNullException("delimiter");
			if (string.IsNullOrEmpty(timeFormat)) throw new ArgumentNullException("timeFormat");
			if (string.IsNullOrEmpty(dateFormat)) throw new ArgumentNullException("dateFormat");

			string date = ParseDate(input[1], dateFormat);
			string time = ParseTime(input[1], timeFormat);

			return date + delimiter + time;
		}

		private string ParseTime(string input, string timeFormat)
		{
			string time = input.Substring(11, 5);

			IFormatProvider format = new DateTimeFormatInfo();
			format.GetFormat(timeFormat.GetType());

			return Convert.ToDateTime(time, format).ToShortTimeString();
		}

		private string ParseDate(string input, string dateFormat)
		{
			string date = input.Substring(0, 4) + input.Substring(5, 2) + input.Substring(8, 2);

			IFormatProvider format = new DateTimeFormatInfo();
			format.GetFormat(dateFormat.GetType());

			return Convert.ToDateTime(date, format).ToShortDateString();
		}
	}
}
