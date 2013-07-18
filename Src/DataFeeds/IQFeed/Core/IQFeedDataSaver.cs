using System;
using System.Collections.Generic;
using System.IO;
using IQFeed.Helpers;
using IQFeed.Models;
using MarketDataDownloader.DomainLogicLayer.Abstraction;
using MarketDataDownloader.DomainLogicLayer.Models;
using System.Globalization;
namespace IQFeed.Core
{
	public class IQFeedDataSaver : IDataFeedSaver
	{
		private readonly IQFeedResponseHelper _responseHelper;
		private  Parameters _parameters;
		private  string _dlm;

		public IQFeedDataSaver(IQFeedResponseHelper responseHelper, Parameters parameters)
		{
			if (responseHelper == null) throw new ArgumentNullException("responseHelper");
			if (parameters == null) throw new ArgumentNullException("parameters");

			_responseHelper = responseHelper;
			_parameters = parameters;
			_dlm = _parameters.OutputDelimiter;
		}

        public void SetProgramParameters(Parameters parameters){
        if (parameters == null) throw new ArgumentNullException("parameters");
            	_parameters = parameters;
                _dlm = _parameters.OutputDelimiter;
        }


        DateTimeFormatInfo GetTimeFormat()
        {
            DateTimeFormatInfo format = new DateTimeFormatInfo();
            format.LongTimePattern = _parameters.TimeFormat;
            format.ShortTimePattern = _parameters.TimeFormat;
            return format;      
        }

        DateTimeFormatInfo GetDateFormat()
        {
            DateTimeFormatInfo format = new DateTimeFormatInfo();
            format.LongDatePattern = _parameters.DateFormat;
            format.ShortDatePattern = _parameters.DateFormat;
            return format; 
        }

        public void SaveData(IQFeedRequest request, ref StreamWriter writer, IList<string> data )
		{
			if (data.Count == 0 || data[0].Substring(0, 1) == "!")
				return;

			if (string.IsNullOrEmpty(_parameters.TimeFormat)) throw new ArgumentNullException("timeFormat");
			DateTimeFormatInfo DateFormat=GetDateFormat();
            if (string.IsNullOrEmpty(_parameters.DateFormat)) throw new ArgumentNullException("dateFormat");
            DateTimeFormatInfo TimeFormat=GetTimeFormat();
            
            
           // using (writer)
			//{
				foreach (var d in data)
				{
					var line = d.Split(',');

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
							response = _responseHelper.ParseTickMarketData(line);
							dateTime = _responseHelper.ParseDateTime(line,
																	_parameters.DateTimeDelimeter,
                                                                    DateFormat,
																	TimeFormat);

							writer.WriteLine( dateTime + _dlm +
											 response.Last + _dlm +
											 response.LastSize + _dlm +
											 response.Bid + _dlm +
											 response.Ask + _dlm +
											 response.BidSize + _dlm +
											 response.AskSize + _dlm+
                                             response.TickId + _dlm +
											 response.TradeType + _dlm );
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
							response = _responseHelper.ParseTickMarketData(line);
							dateTime = _responseHelper.ParseDateTime(line, _parameters.DateTimeDelimeter,DateFormat,TimeFormat);

							writer.WriteLine(dateTime + _dlm +
											 response.Last + _dlm +
											 response.LastSize + _dlm +
											 response.Bid + _dlm +
											 response.Ask + _dlm +
											 response.BidSize + _dlm +
                                             response.AskSize + _dlm + 
                                             response.TickId + _dlm +
                                             response.TradeType + _dlm);
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
							response = _responseHelper.ParseMarketData(line);
							dateTime = _responseHelper.ParseDateTime(line, _parameters.DateTimeDelimeter,
																	DateFormat,
																	TimeFormat);

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
							response = _responseHelper.ParseMarketData(line);
							dateTime = _responseHelper.ParseDateTime(line, _parameters.DateTimeDelimeter,
																	DateFormat,
																TimeFormat);

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
							response = _responseHelper.ParseMarketData(line);
							dateTime = _responseHelper.ParseDateTime(line, _parameters.DateTimeDelimeter,
																	DateFormat,
																	TimeFormat);

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
							response = _responseHelper.ParseMarketData(line);
							dateTime = _responseHelper.ParseDateTime(line, _parameters.DateTimeDelimeter,
															DateFormat,
																	TimeFormat);

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
							response = _responseHelper.ParseMarketData(line);
							dateTime = _responseHelper.ParseDateTime(line, _parameters.DateTimeDelimeter,
																	DateFormat,
																TimeFormat);

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
							response = _responseHelper.ParseMarketData(line);
							dateTime = _responseHelper.ParseDateTime(line, _parameters.DateTimeDelimeter,
																	DateFormat,
																	TimeFormat);

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
			//}
                writer.Flush();
        }
	}
}
