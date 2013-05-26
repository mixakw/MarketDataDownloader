using System;
using System.Globalization;

using IQFeed.Models;

namespace IQFeed.Helpers
{
	public class IQFeedResponseHelper
	{
		public IQFeedResponse ParseTickMarketData(string[] input)
		{
			if (input == null) throw new ArgumentNullException("input");
			if (input.Length != 10) throw new ArgumentException("input.Length");

			IQFeedResponse response = new IQFeedResponse();

			response.RequestId = input[0];
			response.Last = input[2];
			response.LastSize = input[3];
			response.TotalVolume = input[4];
			response.Bid = input[5];
			response.Ask = input[6];
			response.TickId = input[7];
			response.BidSize = input[8];
			response.AskSize = input[9];
			response.TradeType = input[10];

			return response;
		}

		public IQFeedResponse ParseMarketData(string[] input)
		{
			if (input == null) throw new ArgumentNullException("input");
			if (input.Length != 7) throw new ArgumentException("input.Length");

			IQFeedResponse response = new IQFeedResponse();

			response.RequestId = input[0];
			response.High = input[2];
			response.Low = input[3];
			response.Open = input[4];
			response.Close = input[5];
			response.OpenInterest = input[6];
			response.Volume = input[7];

			return response;
		}

		public string ParseDateTime(string[] input, string delimiter, string dateFormat, string timeFormat)
		{
			if (input == null) throw new ArgumentNullException("input");
			if (string.IsNullOrEmpty(delimiter)) throw new ArgumentNullException("delimiter");
			if (string.IsNullOrEmpty(timeFormat)) throw new ArgumentNullException("timeFormat");
			if (string.IsNullOrEmpty(dateFormat)) throw new ArgumentNullException("dateFormat");

			string date = ParseDate(input[1], dateFormat);
			string time = ParseTime(input[1], timeFormat);

			return date + delimiter + time;
		}

		private string ParseDate(string input, string dateFormat)
		{
			string date = input.Substring(0, 4) + input.Substring(5, 2) + input.Substring(8, 2);

			IFormatProvider format = new DateTimeFormatInfo();
			format.GetFormat(dateFormat.GetType());

			return Convert.ToDateTime(date, format).ToShortDateString();
		}

		private string ParseTime(string input, string timeFormat)
		{
			string time = input.Substring(11, 5);

			IFormatProvider format = new DateTimeFormatInfo();
			format.GetFormat(timeFormat.GetType());

			return Convert.ToDateTime(time, format).ToShortTimeString();
		}
	}
}
