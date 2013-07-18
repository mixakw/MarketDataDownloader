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
			if (input.Length != 11) throw new ArgumentException("input.Length");

			IQFeedResponse response = new IQFeedResponse();

			//response.RequestId = input[0];
			response.Last = input[1];
			response.LastSize = input[2];
			response.TotalVolume = input[3];
			response.Bid = input[4];
			response.Ask = input[5];
			response.TickId = input[6];
			response.BidSize = input[7];
			response.AskSize = input[8];
			response.TradeType = input[9];

			return response;
		}

		public IQFeedResponse ParseMarketData(string[] input)
		{
		
            if (input == null) throw new ArgumentNullException("input");
			if (input.Length != 8) throw new ArgumentException("input.Length");

			IQFeedResponse response = new IQFeedResponse();

			//response.RequestId = input[0];
			response.High = input[1];
			response.Low = input[2];
			response.Open = input[3];
			response.Close = input[4];
			response.OpenInterest = input[5];
			response.Volume = input[6];

			return response;
		
        
        }

        public string ParseDateTime(string[] input, string delimiter, DateTimeFormatInfo DateFormat, DateTimeFormatInfo TimeFormat)
		{
			if (input == null) throw new ArgumentNullException("input");
			if (string.IsNullOrEmpty(delimiter)) throw new ArgumentNullException("delimiter");
			

			string date = ParseDate(input[0], DateFormat);
			string time = ParseTime(input[0], TimeFormat);

			return date + delimiter + time;
		}

        private string ParseDate(string input, DateTimeFormatInfo DateFormat)
		{
			//string date = input.Substring(0, 4) + input.Substring(5, 2) + input.Substring(8, 2);

            string date = input.Substring(0, 10);
            DateTime dateValue = DateTime.Parse(date);
            //IFormatProvider format = new DateTimeFormatInfo();
			//format.GetFormat(dateFormat.GetType());
            return dateValue.ToString("d", DateFormat);
			//return Convert.ToDateTime(date, format).ToShortDateString();
		}

        private string ParseTime(string input, DateTimeFormatInfo timeFormat)
		{
            if (input.Length < 11) return "";
                string time = input.Substring(11);

            DateTime dateValue = DateTime.Parse(time);

          //  DateTimeFormatInfo format = new DateTimeFormatInfo();
           // format.LongTimePattern = timeFormat;
           // format.ShortTimePattern = timeFormat;
           // format.GetFormat(timeFormat.GetType());
            return dateValue.ToString("T", timeFormat);
          //  return Convert.ToDateTime(dateValue, format).ToLongTimeString();
		}
	}
}
