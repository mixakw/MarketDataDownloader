using System;
using System.Collections.Generic;
using System.IO;

using IQFeed.Helpers;
using IQFeed.Models;
using MarketDataDownloader.DomainLogicLayer.Models;

namespace IQFeed.Core
{
	public class IQFeedDataSaver
	{
		private readonly IQFeedResponseHelper _responseHelper;
		private readonly Parameters _parameters;
		private readonly string _dlm;

		public IQFeedDataSaver(IQFeedResponseHelper responseHelper, Parameters parameters)
		{
			if (responseHelper == null) throw new ArgumentNullException("responseHelper");
			if (parameters == null) throw new ArgumentNullException("parameters");

			_responseHelper = responseHelper;
			_parameters = parameters;
			_dlm = _parameters.OutputDelimiter;
		}

		public void SaveData(IQFeedRequest request, StreamWriter writer, IList<string> inputData)
		{
			if (inputData.Count == 0 || inputData[0].Substring(0, 1) == "!")
				return;

			using (writer)
			{
				foreach (var input in inputData)
				{
					var data = input.Split(',');

					IQFeedResponse response;
					string dateTime;

					switch (request.TimeFrame)
					{
						//"HTD"
						//Request ID. Text.
						//Time Stamp. Text. Example: 2008-09-01 16:00:01
						//Last. Decimal. Example: 146.2587
						//Last Size. Integer. Example: 100
						//Total Volume. Integer. Example: 1285001
						//Bid. Decimal. Example: 146.2400
						//Ask. Decimal. Example: 146.2600
						//TickID. Integer. Example: 6813524
						//Bid Size. Integer. Example: 100
						//Ask Size. Integer. Example: 100
						//Basis For Last. Character. Current Possible values are 'C' (normal trade) or 'E' (extended trade). 
						case "Tick Days":
							response = _responseHelper.ParseTickMarketData(data);
							dateTime =_responseHelper.ParseDateTime(data, _parameters);

							writer.WriteLine(response.TickId + _dlm +
											 response.TradeType + _dlm +
											 dateTime + _dlm +
											 response.Last + _dlm +
											 response.LastSize + _dlm +
											 response.Bid + _dlm +
											 response.Ask + _dlm +
											 response.BidSize + _dlm +
											 response.AskSize + _dlm);
							break;

						//"HTT"
						//Request ID. Text.
						//Time Stamp. Text. Example: 2008-09-01 16:00:01
						//Last. Decimal. Example: 146.2587
						//Last Size. Integer. Example: 100
						//Total Volume. Integer. Example: 1285001
						//Bid. Decimal. Example: 146.2400
						//Ask. Decimal. Example: 146.2600
						//TickID. Integer. Example: 6813524
						//Bid Size. Integer. Example: 100
						//Ask Size. Integer. Example: 100
						//Basis For Last. Character. Current Possible values are 'C' (normal trade) or 'E' (extended trade).
						case "Tick Interval":
							response = _responseHelper.ParseTickMarketData(data);
							dateTime = _responseHelper.ParseDateTime(data, _parameters);

							writer.WriteLine(response.TickId + _dlm +
											 response.TradeType + _dlm +
											 dateTime + _dlm +
											 response.Last + _dlm +
											 response.LastSize + _dlm +
											 response.Bid + _dlm +
											 response.Ask + _dlm +
											 response.BidSize + _dlm +
											 response.AskSize + _dlm);
							break;

						//"HID"
						//Request ID. Text.
						//Time Stamp. CCYY-MM-DD HH:MM:SS	Example: 2008-09-01 16:00:01
						//High. Decimal. Example: 146.2587
						//Low. Decimal. Example: 145.2587
						//Open. Decimal. Example: 146.2587
						//Close. Decimal. Example: 145.2587
						//Total Volume.	Integer. Example: 1285001
						//Period Volume. Integer. Example: 1285
						case "Intraday Days":
							response = _responseHelper.ParseMarketData(data);
							dateTime = _responseHelper.ParseDateTime(data, _parameters);

							writer.WriteLine(dateTime + _dlm +
											 response.Open + _dlm +
											 response.High + _dlm +
											 response.Low + _dlm +
											 response.Close + _dlm +
											 response.Volume + _dlm);
							break;

						//"HIT"
						//Request ID. Text.
						//Time Stamp. CCYY-MM-DD HH:MM:SS	Example: 2008-09-01 16:00:01
						//High. Decimal. Example: 146.2587
						//Low. Decimal. Example: 145.2587
						//Open. Decimal. Example: 146.2587
						//Close. Decimal. Example: 145.2587
						//Total Volume.	Integer. Example: 1285001
						//Period Volume. Integer. Example: 1285
						case "Intraday Interval":
							response = _responseHelper.ParseMarketData(data);
							dateTime = _responseHelper.ParseDateTime(data, _parameters);

							writer.WriteLine(dateTime + _dlm +
											 response.Open + _dlm +
											 response.High + _dlm +
											 response.Low + _dlm +
											 response.Close + _dlm +
											 response.Volume + _dlm);
							break;

						//"HDX"
						//Request ID. Text.
						//Time Stamp. CCYY-MM-DD HH:MM:SS. Example: 2008-09-01 16:00:01
						//High. Decimal. Example: 146.2587
						//Low. Decimal. Example: 145.2587
						//Open. Decimal. Example: 146.2587
						//Close. Decimal. Example: 145.2587
						//Period Volume. Integer. Example: 1285001
						//Open Interest. Integer. Example: 128
						case "Daily Days":
							response = _responseHelper.ParseMarketData(data);
							dateTime = _responseHelper.ParseDateTime(data, _parameters);

							writer.WriteLine(dateTime + _dlm +
											 response.Open + _dlm +
											 response.High + _dlm +
											 response.Low + _dlm +
											 response.Close + _dlm +
											 response.Volume + _dlm +
											 response.OpenInterest + _dlm);
							break;

						//"HDT"
						//Request ID. Text.
						//Time Stamp. CCYY-MM-DD HH:MM:SS. Example: 2008-09-01 16:00:01
						//High. Decimal. Example: 146.2587
						//Low. Decimal. Example: 145.2587
						//Open. Decimal. Example: 146.2587
						//Close. Decimal. Example: 145.2587
						//Period Volume. Integer. Example: 1285001
						//Open Interest. Integer. Example: 128
						case "Daily Interval":
							response = _responseHelper.ParseMarketData(data);
							dateTime = _responseHelper.ParseDateTime(data, _parameters);

							writer.WriteLine(dateTime + _dlm +
											 response.Open + _dlm +
											 response.High + _dlm +
											 response.Low + _dlm +
											 response.Close + _dlm +
											 response.Volume + _dlm +
											 response.OpenInterest + _dlm);
							break;

						//"HWX"
						//Request ID. Text.
						//Time Stamp. CCYY-MM-DD HH:MM:SS. Example: 2008-09-01 16:00:01
						//High. Decimal. Example: 146.2587
						//Low. Decimal. Example: 145.2587
						//Open. Decimal. Example: 146.2587
						//Close. Decimal. Example: 145.2587
						//Period Volume. Integer. Example: 1285001
						//Open Interest. Integer. Example: 128
						case "Weekly Days":
							response = _responseHelper.ParseMarketData(data);
							dateTime = _responseHelper.ParseDateTime(data, _parameters);

							writer.WriteLine(dateTime + _dlm +
											 response.Open + _dlm +
											 response.High + _dlm +
											 response.Low + _dlm +
											 response.Close + _dlm +
											 response.Volume + _dlm +
											 response.OpenInterest + _dlm);
							break;

						//"HMX"
						//Request ID. Text.
						//Time Stamp. CCYY-MM-DD HH:MM:SS. Example: 2008-09-01 16:00:01
						//High. Decimal. Example: 146.2587
						//Low. Decimal. Example: 145.2587
						//Open. Decimal. Example: 146.2587
						//Close. Decimal. Example: 145.2587
						//Period Volume. Integer. Example: 1285001
						//Open Interest. Integer. Example: 128
						case "Monthly Days":
							response = _responseHelper.ParseMarketData(data);
							dateTime = _responseHelper.ParseDateTime(data, _parameters);

							writer.WriteLine(dateTime + _dlm +
											 response.Open + _dlm +
											 response.High + _dlm +
											 response.Low + _dlm +
											 response.Close + _dlm +
											 response.Volume + _dlm +
											 response.OpenInterest + _dlm);
							break;
					}
				}
			}
		}
	}
}
