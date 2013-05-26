// =================================================
// File:
// MarketDataDownloader/IQFeed/IQFeedQueryBuilder.cs
// 
// Last updated:
// 2013-05-24 4:34 PM
// =================================================

#region Usings

using System;

using MarketDataDownloader.DomainLogicLayer.Abstraction;
using MarketDataDownloader.Logging;

#endregion

namespace IQFeed.Core
{
	public class IQFeedQueryBuilder : IDataFeedQueryBuilder
	{
		private readonly IMyLogger _logger;

		public IQFeedQueryBuilder(IMyLogger logger)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			_logger = logger;
		}

		public string CreateQuery(IRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}

			var dlr = IQFeedConfiguration.Delimiter;
			var trm = IQFeedConfiguration.Terminater;

			var query = string.Empty;

			switch (request.TimeFrame)
			{
				case "Tick Days":
					//HTD,[Symbol],[Days],[MaxDatapoints],[BeginFilterTime],[EndFilterTime],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves ticks for the previous [Days] days for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[Days] - Required - The number of calendar days ("1" equals current day) of tick history to be retrieved
					//[MaxDatapoints] - Optional - the maximum number of data points to be retrieved.
					//[BeginFilterTime] - Optional - Format HHmmSS - Allows you to specify the earliest time of day (EST) for which to receive data.
					//[EndFilterTime] - Optional - Format HHmmSS - Allows you to specify the latest time of day (EST) for which to receive data.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app.
					query = 
						IQFeedConfiguration.TickDaysHeader + dlr + 
						request.CurrentSymbol + dlr + 
						request.Days + dlr +
						request.MaxDatapoints + dlr + 
						request.BeginFilterTime + dlr + 
						request.EndFilterTime + dlr + request.DataDirection + dlr + 
						request.DatapointsPerSend + trm;

					_logger.Info(query);
					break;

				case "Tick Interval":
					//HTT,[Symbol],[BeginDate BeginTime],[EndDate EndTime],[MaxDatapoints],[BeginFilterTime],[EndFilterTime],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves tick data between [BeginDate BeginTime] and [EndDate EndTime] for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[BeginDate BeginTime] - Required if [EndDate EndTime] not specified - Format CCYYMMDD HHmmSS - Earliest date/time to receive data for.
					//[EndDate EndTime] - Required if [BeginDate BeginTime] not specified - Format CCYYMMDD HHmmSS - Most recent date/time to receive data for.
					//[MaxDatapoints] - Optional - the maximum number of data points to be retrieved.
					//[BeginFilterTime] - Optional - Format HHmmSS - Allows you to specify the earliest time of day (EST) for which to receive data.
					//[EndFilterTime] - Optional - Format HHmmSS - Allows you to specify the latest time of day (EST) for which to receive data.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					query = 
						IQFeedConfiguration.TickIntervalHeader + dlr + 
						request.CurrentSymbol + dlr + 
						request.BeginDateTime + dlr +
						request.EndDateTime + dlr + 
						request.MaxDatapoints + dlr + 
						request.BeginFilterTime + dlr + 
						request.EndFilterTime + dlr + 
						request.DataDirection + dlr + 
						request.DatapointsPerSend + trm;

					_logger.Info(query);
					break;

				case "Intraday Days":
					//HID,[Symbol],[Interval],[Days],[MaxDatapoints],[BeginFilterTime],[EndFilterTime],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves [Days] days of interval data for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[Interval] - Required - The interval in seconds.
					//[Days] - Required - The number of calendar days ("1" equals current day) of tick history to be retrieved
					//[MaxDatapoints] - Optional - the maximum number of data points to be retrieved.
					//[BeginFilterTime] - Optional - Format HHmmSS - Allows you to specify the earliest time of day (EST) for which to receive data.
					//[EndFilterTime] - Optional - Format HHmmSS - Allows you to specify the latest time of day (EST) for which to receive data.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					query = 
						IQFeedConfiguration.IntradayDaysHeader + dlr + 
						request.CurrentSymbol + dlr + 
						request.Interval + dlr +
						request.Days + dlr + 
						request.DatapointsPerSend + dlr + 
						request.BeginFilterTime + dlr + 
						request.EndFilterTime + dlr + 
						request.DataDirection + dlr + 
						request.DatapointsPerSend + trm;

					_logger.Info(query);
					break;

				case "Intraday Interval":
					//HIT,[Symbol],[Interval],[BeginDate BeginTime],[EndDate EndTime],[MaxDatapoints],[BeginFilterTime],[EndFilterTime],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves interval data between [BeginDate BeginTime] and [EndDate EndTime] for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[Interval] - Required - The interval in seconds.
					//[BeginDate BeginTime] - Required if [EndDate EndTime] not specified - Format CCYYMMDD HHmmSS - Earliest date/time to receive data for.
					//[EndDate EndTime] - Required if [BeginDate BeginTime] not specified - Format CCYYMMDD HHmmSS - Most recent date/time to receive data for.
					//[MaxDatapoints] - Optional - the maximum number of data points to be retrieved.
					//[BeginFilterTime] - Optional - Format HHmmSS - Allows you to specify the earliest time of day (EST) for which to receive data.
					//[EndFilterTime] - Optional - Format HHmmSS - Allows you to specify the latest time of day (EST) for which to receive data.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					query = 
						IQFeedConfiguration.IntradayIntervalHeader + dlr + 
						request.CurrentSymbol + dlr + 
						request.Interval + dlr +
						request.BeginDateTime + dlr + 
						request.EndDateTime + dlr + 
						request.MaxDatapoints + dlr + 
						request.BeginFilterTime +
						dlr + request.EndFilterTime + dlr + 
						request.DataDirection + dlr + 
						request.DatapointsPerSend + trm;

					_logger.Info(query);
					break;

				case "Daily Days":
					//HDX,[Symbol],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves up to [MaxDatapoints] number of End-Of-Day Data for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[MaxDatapoints] - Required - The maximum number of data points to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					query = 
						IQFeedConfiguration.DailyDaysHeader + dlr + 
						request.CurrentSymbol + dlr + 
						request.DatapointsPerSend + dlr +
						request.DataDirection + dlr + 
						request.DatapointsPerSend + trm;

					_logger.Info(query);
					break;

				case "Daily Interval":
					//HDT,[Symbol],[BeginDate],[EndDate],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves Daily data between [BeginDate] and [EndDate] for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[BeginDate] - Required if [EndDate] not specified - Format CCYYMMDD - Earliest date to receive data for.
					//[EndDate] - Required if [BeginDate] not specified - Format CCYYMMDD - Most recent date to receive data for.
					//[MaxDatapoints] - Optional - the maximum number of data points to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					query = 
						IQFeedConfiguration.DailyIntervalHeader + dlr + 
						request.CurrentSymbol + dlr + 
						request.BeginDateTime + dlr +
						request.EndDateTime + dlr + 
						request.MaxDatapoints + dlr + 
						request.DataDirection + dlr + 
						request.DatapointsPerSend + trm;

					_logger.Info(query);
					break;

				case "Weekly Days":
					//HWX,[Symbol],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves up to [MaxDatapoints] data points of composite weekly data points for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[MaxDatapoints] - Required - The maximum number of data points to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					query = 
						IQFeedConfiguration.WeeklyDaysHeader + dlr + 
						request.CurrentSymbol + dlr + 
						request.MaxDatapoints + dlr +
						request.DataDirection + dlr + 
						request.DatapointsPerSend + trm;

					_logger.Info(query);
					break;

				case "Monthly Days":
					//HMX,[Symbol],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves up to [MaxDatapoints] data points of composite monthly data points for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[MaxDatapoints] - Required - The maximum number of data points to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					query = 
						IQFeedConfiguration.MonthlyDaysHeader + dlr + 
						request.CurrentSymbol + dlr + 
						request.MaxDatapoints + dlr +
						request.DataDirection + dlr + 
						request.DatapointsPerSend + trm;

					_logger.Info(query);
					break;

				default:
					_logger.Error("Request error");
					break;
			}

			return query;
		}
	}
}
