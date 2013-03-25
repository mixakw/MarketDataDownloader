using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

using log4net;

namespace IQFeedDownloader
{
	public class Helper
	{
		#region Constants

		private const string NoDataError = "NO_DATA";
		private const string EndMessage = "ENDMSG";
		private const string SyntaxError = "SYNTAX_ERROR";
		private const string InvalidSymbolError = "Invalid symbol";

		private const string IQFeedHostName = "127.0.0.1";
		private const int IQFeedHistoryPort = 9100;

		private const byte EOL = 0x0;

		#endregion

		#region Variables

		private static readonly ILog Logger = LogManager.GetLogger(typeof(Helper));
		private TcpClient _socket;
		private NetworkStream _networkStream;

		#endregion

		#region Public Properties

		public TcpClient Socket
		{
			get { return _socket; }
		}

		#endregion Public Properties

		#region Public Methods

		public void OpenConnection()
		{
			try
			{
				_socket = new TcpClient(IQFeedHostName, IQFeedHistoryPort);
			}
			catch (SocketException)
			{

			}
		}

		public string CreateQuery(Request r)
		{
			if (r == null) throw new ArgumentNullException("r");

			switch (r.TimeFrame)
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
					return r.TickDaysHeader + r.Delimiter +
							r.CurrentSymbol + r.Delimiter +
							r.Days + r.Delimiter +
							r.MaxDatapoints + r.Delimiter +
							r.BeginFilterTime + r.Delimiter +
							r.EndFilterTime + r.Delimiter +
							r.DataDirection + r.Delimiter +
							r.DatapointsPerSend + r.TerminatingCharacter;

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
					return r.TickIntervalHeader + r.Delimiter +
							r.CurrentSymbol + r.Delimiter +
							r.BeginDateTime + r.Delimiter +
							r.EndDateTime + r.Delimiter +
							r.MaxDatapoints + r.Delimiter +
							r.BeginFilterTime + r.Delimiter +
							r.EndFilterTime + r.Delimiter +
							r.DataDirection + r.Delimiter +
							r.DatapointsPerSend + r.TerminatingCharacter;

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
					return r.IntradayDaysHeader + r.Delimiter +
							r.CurrentSymbol + r.Delimiter +
							r.Interval + r.Delimiter +
							r.Days + r.Delimiter +
							r.DatapointsPerSend + r.Delimiter +
							r.BeginFilterTime + r.Delimiter +
							r.EndFilterTime + r.Delimiter +
							r.DataDirection + r.Delimiter +
							r.DatapointsPerSend + r.TerminatingCharacter;

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
					return r.IntradayIntervalHeader + r.Delimiter +
							r.CurrentSymbol + r.Delimiter +
							r.Interval + r.Delimiter +
							r.BeginDateTime + r.Delimiter +
							r.EndDateTime + r.Delimiter +
							r.MaxDatapoints + r.Delimiter +
							r.BeginFilterTime + r.Delimiter +
							r.EndFilterTime + r.Delimiter +
							r.DataDirection + r.Delimiter +
							r.DatapointsPerSend + r.TerminatingCharacter;

				case "Daily Days":
					//HDX,[Symbol],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves up to [MaxDatapoints] number of End-Of-Day Data for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[MaxDatapoints] - Required - The maximum number of data points to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					return r.DailyDaysHeader + r.Delimiter +
							r.CurrentSymbol + r.Delimiter +
							r.DatapointsPerSend + r.Delimiter +
							r.DataDirection + r.Delimiter +
							r.DatapointsPerSend + r.TerminatingCharacter;

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
					return r.DailyIntervalHeader + r.Delimiter +
							r.CurrentSymbol + r.Delimiter +
							r.BeginDateTime + r.Delimiter +
							r.EndDateTime + r.Delimiter +
							r.MaxDatapoints + r.Delimiter +
							r.DataDirection + r.Delimiter +
							r.DatapointsPerSend + r.TerminatingCharacter;

				case "Weekly Days":
					//HWX,[Symbol],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves up to [MaxDatapoints] data points of composite weekly data points for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[MaxDatapoints] - Required - The maximum number of data points to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					return r.WeeklyDaysHeader + r.Delimiter +
							r.CurrentSymbol + r.Delimiter +
							r.MaxDatapoints + r.Delimiter +
							r.DataDirection + r.Delimiter +
							r.DatapointsPerSend + r.TerminatingCharacter;

				case "Monthly Days":
					//HMX,[Symbol],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves up to [MaxDatapoints] data points of composite monthly data points for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[MaxDatapoints] - Required - The maximum number of data points to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					return r.MonthlyDaysHeader + r.Delimiter +
							r.CurrentSymbol + r.Delimiter +
							r.MaxDatapoints + r.Delimiter +
							r.DataDirection + r.Delimiter +
							r.DatapointsPerSend + r.TerminatingCharacter;

				default:
					Logger.Error("Request error");
					return string.Empty;
			}
		}

		public void GetData(string request, string folder, Request r)
		{
			if (string.IsNullOrEmpty(request)) throw new ArgumentNullException("request");

			if (CreateStream())
			{
				SendRequest(request);

				StreamReader streamReader = new StreamReader(_networkStream);

				if (IsValidContent(streamReader))
				{
					int index = 0;
					int accumulationIndex = 0;
					string data = string.Empty;
					List<string> inboundData = new List<string>();

					while (true)
					{
						data = streamReader.ReadLine();

						if (!data.Contains(EndMessage))
						{
							inboundData.Add(data);
							index++;

							if (index == 100000)
							{
								accumulationIndex = accumulationIndex + index;
								Logger.Info(String.Format("{0} entries of data processed", accumulationIndex));
								ParseData(r, folder, inboundData);
								inboundData.Clear();
								index = 0;
							}
						}
					}

					if (index == 0) return;

					Logger.Info(String.Format("{0} entries of data processed", index));

					streamWriter = GetWriterToFile(folder, symbol);
					ParseData(timeframeName, inboundData, streamWriter);
					streamWriter.Close();

					inboundData.Clear();
				}
			}

		}

		#endregion

		#region Private Methods

		private bool CreateStream()
		{
			bool isSuccessful = true;

			if (Socket == null)
				OpenConnection();

			_networkStream = Socket.GetStream();

			if (!_networkStream.CanRead && !_networkStream.CanWrite)
			{
				Logger.Error("You cannot read/write data from this stream.");
				isSuccessful = false;
				_networkStream.Close();
			}

			return isSuccessful;
		}

		private void SendRequest(string request)
		{
			var sendBytes = Encoding.UTF8.GetBytes(request + EOL);
			_networkStream.Write(sendBytes, 0, sendBytes.Length);
		}

		private StreamWriter GetWriterToFile(string folder, string symbol)
		{
			var quotesfilepath = folder + @"\" + symbol + ".txt";
			var fs = new FileStream(quotesfilepath, FileMode.Append, FileAccess.Write);
			return new StreamWriter(fs);
		}

		private bool IsValidContent(StreamReader reader)
		{
			bool isValidContent = true;

			string line = reader.ReadLine();

			if (line.Contains(NoDataError))
			{
				isValidContent = false;
				Logger.Error("No data");
			}

			if (line.Contains(InvalidSymbolError))
			{
				isValidContent = false;
				Logger.Error("Invalid symbol");
			}

			if (line.Contains(SyntaxError))
			{
				isValidContent = false;
				Logger.Error("Syntax error");
			}

			return isValidContent;
		}

		private void ParseData(Request r, string folder, List<string> inboundData)
		{
			if (inboundData.Count == 0 || inboundData[0].Substring(0, 1) == "!")
				return;

			using (StreamWriter writerToFile = GetWriterToFile(folder, r.CurrentSymbol))
			{
				foreach (var data in inboundData)
				{
					var splittedData = data.Split(',');
					var response = new Response();

					switch (r.TimeFrame)
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
							response.ParseMarketData(splittedData, true);
							response.ParseDateTime(splittedData, DateFormat, TimeFormat);

							writerToFile.WriteLine(
								response.TickId + r.Delimiter +
								response.TradeType + r.Delimiter +
								response.FormattedDateTime(DateTimeSeparator) +
								",{0},{1},{2},{3},{4},{5}",
								response.Last,
								response.LastSize,
								response.Bid,
								response.Ask,
								response.BidSize,
								response.AskSize);
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
							response.ParseMarketData(splittedData, true);
							response.ParseDateTime(splittedData, DateFormat, TimeFormat);

							writerToFile.WriteLine(
								response.TickId + r.Delimiter +
								response.TradeType + r.Delimiter +
								response.FormattedDateTime(DateTimeSeparator) +
								",{0},{1},{2},{3},{4},{5}",
								response.Last,
								response.LastSize,
								response.Bid,
								response.Ask,
								response.BidSize,
								response.AskSize);
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
							response.ParseMarketData(splittedData, false);
							response.ParseDateTime(splittedData, DateFormat, TimeFormat);

							writerToFile.WriteLine(
								response.FormattedDateTime(DateTimeSeparator) +
								",{0},{1},{2},{3},{4}",
								response.Open,
								response.High,
								response.Low,
								response.Close,
								response.Volume);
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
							response.ParseMarketData(splittedData, false);
							response.ParseDateTime(splittedData, DateFormat, TimeFormat);

							writerToFile.WriteLine(
								response.FormattedDateTime(DateTimeSeparator) +
								",{0},{1},{2},{3},{4}",
								response.Open,
								response.High,
								response.Low,
								response.Close,
								response.Volume);
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
							response.ParseMarketData(splittedData, false);
							response.ParseDateTime(splittedData, DateFormat, TimeFormat);

							writerToFile.WriteLine(
								response.FormattedDate +
								",{0},{1},{2},{3},{4},{5}",
								response.Open,
								response.High,
								response.Low,
								response.Close,
								response.Volume,
								response.OpenInterest);
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
							response.ParseMarketData(splittedData, false);
							response.ParseDateTime(splittedData, DateFormat, TimeFormat);

							writerToFile.WriteLine(
								response.FormattedDate +
								",{0},{1},{2},{3},{4},{5}",
								response.Open,
								response.High,
								response.Low,
								response.Close,
								response.Volume,
								response.OpenInterest);
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
							response.ParseMarketData(splittedData, false);
							response.ParseDateTime(splittedData, DateFormat, TimeFormat);

							writerToFile.WriteLine(
								response.FormattedDate +
								",{0},{1},{2},{3},{4},{5}",
								response.Open,
								response.High,
								response.Low,
								response.Close,
								response.Volume,
								response.OpenInterest);
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
							response.ParseMarketData(splittedData, false);
							response.ParseDateTime(splittedData, DateFormat, TimeFormat);

							writerToFile.WriteLine(
								response.FormattedDate +
								",{0},{1},{2},{3},{4},{5}",
								response.Open,
								response.High,
								response.Low,
								response.Close,
								response.Volume,
								response.OpenInterest);
							break;
					}
				}
			}
		}

		#endregion
	}
}