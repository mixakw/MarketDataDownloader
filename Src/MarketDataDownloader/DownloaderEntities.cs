using System;
using System.Globalization;

namespace MarketDataDownloader
{
	internal sealed class DownloaderEntities
	{
		#region Public Properties

		public string RequestId { get; set; }
		public string Year { get; set; }
		public string Month { get; set; }
		public string Day { get; set; }
		public string Time { get; set; }
		public decimal High { get; set; }
		public decimal Low { get; set; }
		public decimal Open { get; set; }
		public decimal Close { get; set; }
		public long Volume { get; set; }
		public long OpenInterest { get; set; }
		public decimal Last { get; set; }
		public long LastSize { get; set; }
		public long TotalVolume { get; set; }
		public decimal Bid { get; set; }
		public decimal Ask { get; set; }
		public long TickId { get; set; }
		public long BidSize { get; set; }
		public long AskSize { get; set; }
		public char TradeType { get; set; }

		#endregion

		#region Convert Methods

		public string HighAsString()
		{
			return High.ToString(CultureInfo.InvariantCulture);
		}

		public string OpenAsString()
		{
			return Open.ToString(CultureInfo.InvariantCulture);
		}

		public string LowAsString()
		{
			return Low.ToString(CultureInfo.InvariantCulture);
		}

		public string CloseAsString()
		{
			return Close.ToString(CultureInfo.InvariantCulture);
		}

		public string LastAsString()
		{
			return Last.ToString(CultureInfo.InvariantCulture);
		}

		public string BidAsString()
		{
			return Bid.ToString(CultureInfo.InvariantCulture);
		}

		public string AskAsString()
		{
			return Ask.ToString(CultureInfo.InvariantCulture);
		}

		#endregion

		#region Public Methods

		public void ParseData(string[] inputData, bool isTickData)
		{
			if (inputData == null) throw new ArgumentNullException("inputData");

			RequestId = inputData[0];
			
			Year = inputData[1].Substring(0, 4);
			Month = inputData[1].Substring(5, 2);
			Day = inputData[1].Substring(8, 2);
			Time = inputData[1].Substring(11, 5);

			if (isTickData)
			{
				Last = decimal.Parse(inputData[2], CultureInfo.InvariantCulture);
				LastSize = long.Parse(inputData[3]);
				TotalVolume = long.Parse(inputData[4]);
				Bid = decimal.Parse(inputData[5], CultureInfo.InvariantCulture);
				Ask = decimal.Parse(inputData[6], CultureInfo.InvariantCulture);
				TickId = long.Parse(inputData[7]);
				BidSize = long.Parse(inputData[8]);
				AskSize = long.Parse(inputData[9]);
				TradeType = char.Parse(inputData[10]);
				return;
			}

			High = decimal.Parse(inputData[2], CultureInfo.InvariantCulture);
			Low = decimal.Parse(inputData[3], CultureInfo.InvariantCulture);
			Open = decimal.Parse(inputData[4], CultureInfo.InvariantCulture);
			Close = decimal.Parse(inputData[5], CultureInfo.InvariantCulture);
			OpenInterest = long.Parse(inputData[6]);
			Volume = long.Parse(inputData[7]);
		}

		#endregion
	}
}
