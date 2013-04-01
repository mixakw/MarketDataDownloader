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
		private const string NoDataError = "NO_DATA";
		private const string EndMessage = "ENDMSG";
		private const string SyntaxError = "SYNTAX_ERROR";
		private const string InvalidSymbolError = "Invalid symbol";

		private const string IQFeedHostName = "127.0.0.1";
		private const int IQFeedHistoryPort = 9100;

		private const byte EOL = 0x0;

		private ILog _logger;
		private TcpClient _socket;
		private NetworkStream _networkStream;

		private Parameters _parameters;
		private Response _response;

		public Helper()
		{
			_parameters = new Parameters();
			_response = new Response();
			_logger = LogManager.GetLogger(typeof(Helper));
		}

		public TcpClient Socket
		{
			get { return _socket; }
		}

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

		public string CreateQuery(Request request)
		{
			if (request == null) throw new ArgumentNullException("request");

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
					return request.TickDaysHeader + request.Delimiter +
						   request.CurrentSymbol + request.Delimiter +
						   request.Days + request.Delimiter +
						   request.MaxDatapoints + request.Delimiter +
						   request.BeginFilterTime + request.Delimiter +
						   request.EndFilterTime + request.Delimiter +
						   request.DataDirection + request.Delimiter +
						   request.DatapointsPerSend + request.Terminater;

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
					return request.TickIntervalHeader + request.Delimiter +
						   request.CurrentSymbol + request.Delimiter +
						   request.BeginDateTime + request.Delimiter +
						   request.EndDateTime + request.Delimiter +
						   request.MaxDatapoints + request.Delimiter +
						   request.BeginFilterTime + request.Delimiter +
						   request.EndFilterTime + request.Delimiter +
						   request.DataDirection + request.Delimiter +
						   request.DatapointsPerSend + request.Terminater;

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
					return request.IntradayDaysHeader + request.Delimiter +
						   request.CurrentSymbol + request.Delimiter +
						   request.Interval + request.Delimiter +
						   request.Days + request.Delimiter +
						   request.DatapointsPerSend + request.Delimiter +
						   request.BeginFilterTime + request.Delimiter +
						   request.EndFilterTime + request.Delimiter +
						   request.DataDirection + request.Delimiter +
						   request.DatapointsPerSend + request.Terminater;

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
					return request.IntradayIntervalHeader + request.Delimiter +
						   request.CurrentSymbol + request.Delimiter +
						   request.Interval + request.Delimiter +
						   request.BeginDateTime + request.Delimiter +
						   request.EndDateTime + request.Delimiter +
						   request.MaxDatapoints + request.Delimiter +
						   request.BeginFilterTime + request.Delimiter +
						   request.EndFilterTime + request.Delimiter +
						   request.DataDirection + request.Delimiter +
						   request.DatapointsPerSend + request.Terminater;

				case "Daily Days":
					//HDX,[Symbol],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves up to [MaxDatapoints] number of End-Of-Day Data for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[MaxDatapoints] - Required - The maximum number of data points to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					return request.DailyDaysHeader + request.Delimiter +
						   request.CurrentSymbol + request.Delimiter +
						   request.DatapointsPerSend + request.Delimiter +
						   request.DataDirection + request.Delimiter +
						   request.DatapointsPerSend + request.Terminater;

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
					return request.DailyIntervalHeader + request.Delimiter +
						   request.CurrentSymbol + request.Delimiter +
						   request.BeginDateTime + request.Delimiter +
						   request.EndDateTime + request.Delimiter +
						   request.MaxDatapoints + request.Delimiter +
						   request.DataDirection + request.Delimiter +
						   request.DatapointsPerSend + request.Terminater;

				case "Weekly Days":
					//HWX,[Symbol],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves up to [MaxDatapoints] data points of composite weekly data points for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[MaxDatapoints] - Required - The maximum number of data points to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					return request.WeeklyDaysHeader + request.Delimiter +
						   request.CurrentSymbol + request.Delimiter +
						   request.MaxDatapoints + request.Delimiter +
						   request.DataDirection + request.Delimiter +
						   request.DatapointsPerSend + request.Terminater;

				case "Monthly Days":
					//HMX,[Symbol],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves up to [MaxDatapoints] data points of composite monthly data points for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[MaxDatapoints] - Required - The maximum number of data points to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					return request.MonthlyDaysHeader + request.Delimiter +
						   request.CurrentSymbol + request.Delimiter +
						   request.MaxDatapoints + request.Delimiter +
						   request.DataDirection + request.Delimiter +
						   request.DatapointsPerSend + request.Terminater;

				default:
					_logger.Error("Request error");
					return string.Empty;
			}
		}

		public void GetData(string request, string folder, Request input)
		{
			if (string.IsNullOrEmpty(request)) throw new ArgumentNullException("request");

			if (CreateStream())
			{
				SendRequest(request);

				using (var streamReader = new StreamReader(_networkStream))
				{
					if (IsValidContent(streamReader))
					{
						int index = 0;
						int accumulationIndex = 0;
						var inboundData = new List<string>();

						while (true)
						{
							string data = streamReader.ReadLine();

							if (data != null && !data.Contains(EndMessage))
							{
								inboundData.Add(data);
								index++;

								if (index == 100000)
								{
									accumulationIndex = accumulationIndex + index;
									_logger.Info(string.Format("{0} entries of data processed", accumulationIndex));
									ParseData(input, folder, inboundData);
									inboundData.Clear();
									index = 0;
								}
							}

							if (index != 0)
							{
								_logger.Info(String.Format("{0} entries of data processed", index));
								ParseData(input, folder, inboundData);
							}

							inboundData.Clear();
						}
					}
				}
			}
		}

		#region Private Methods

		private bool CreateStream()
		{
			bool isSuccessful = true;

			if (Socket == null)
				OpenConnection();

			_networkStream = Socket.GetStream();

			if (!_networkStream.CanRead && !_networkStream.CanWrite)
			{
				_logger.Error("You cannot read/write data from this stream.");
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

		private bool IsValidContent(TextReader reader)
		{
			bool isValidContent = true;

			string line = reader.ReadLine();

			if (line != null)
			{
				if (line.Contains(NoDataError))
				{
					isValidContent = false;
					_logger.Error("No data");
				}

				if (line.Contains(InvalidSymbolError))
				{
					isValidContent = false;
					_logger.Error("Invalid symbol");
				}

				if (line.Contains(SyntaxError))
				{
					isValidContent = false;
					_logger.Error("Syntax error");
				}
			}

			return isValidContent;
		}

		private void ParseData(Request request, string folder, IList<string> inputData)
		{
			if (inputData.Count == 0 || inputData[0].Substring(0, 1) == "!")
				return;

			using (StreamWriter writerToFile = GetWriterToFile(folder, request.CurrentSymbol))
			{
				foreach (var input in inputData)
				{
					var data = input.Split(',');

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
							_response.ParseTickMarketData(data);
							_response.ParseDateTime(data);

							writerToFile.WriteLine(_response.TickId +_parameters.OutputDelimiter +
												   _response.TradeType +_parameters.OutputDelimiter +
												   _response.ResponseDateTime + _parameters.OutputDelimiter +
												   _response.Last+ _parameters.OutputDelimiter +
												   _response.LastSize+ _parameters.OutputDelimiter +
												   _response.Bid+ _parameters.OutputDelimiter +
												   _response.Ask+ _parameters.OutputDelimiter +
												   _response.BidSize+ _parameters.OutputDelimiter +
												   _response.AskSize+ _parameters.OutputDelimiter);
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
							_response.ParseTickMarketData(data);
							_response.ParseDateTime(data);

							writerToFile.WriteLine(_response.TickId + _parameters.OutputDelimiter +
												   _response.TradeType + _parameters.OutputDelimiter +
												   _response.ResponseDateTime + _parameters.OutputDelimiter +
												   _response.Last + _parameters.OutputDelimiter +
												   _response.LastSize + _parameters.OutputDelimiter +
												   _response.Bid + _parameters.OutputDelimiter +
												   _response.Ask + _parameters.OutputDelimiter +
												   _response.BidSize + _parameters.OutputDelimiter +
												   _response.AskSize + _parameters.OutputDelimiter);
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
							_response.ParseMarketData(data);
							_response.ParseDateTime(data);

							writerToFile.WriteLine(_response.ResponseDateTime + _parameters.OutputDelimiter +
													_response.Open + _parameters.OutputDelimiter +
													_response.High + _parameters.OutputDelimiter +
													_response.Low + _parameters.OutputDelimiter +
													_response.Close + _parameters.OutputDelimiter +
													_response.Volume + _parameters.OutputDelimiter);
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
							_response.ParseMarketData(data);
							_response.ParseDateTime(data);

							writerToFile.WriteLine(_response.ResponseDateTime + _parameters.OutputDelimiter +
															_response.Open + _parameters.OutputDelimiter +
															_response.High + _parameters.OutputDelimiter +
															_response.Low + _parameters.OutputDelimiter +
															_response.Close + _parameters.OutputDelimiter +
															_response.Volume + _parameters.OutputDelimiter);
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
							_response.ParseMarketData(data);
							_response.ParseDateTime(data);

							writerToFile.WriteLine(_response.ResponseDate + _parameters.OutputDelimiter +
													_response.Open + _parameters.OutputDelimiter +
													_response.High + _parameters.OutputDelimiter +
													_response.Low + _parameters.OutputDelimiter +
													_response.Close + _parameters.OutputDelimiter +
													_response.Volume + _parameters.OutputDelimiter +
													_response.OpenInterest + _parameters.OutputDelimiter);
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
							_response.ParseMarketData(data);
							_response.ParseDateTime(data);

							writerToFile.WriteLine(_response.ResponseDate + _parameters.OutputDelimiter +
													_response.Open + _parameters.OutputDelimiter +
													_response.High + _parameters.OutputDelimiter +
													_response.Low + _parameters.OutputDelimiter +
													_response.Close + _parameters.OutputDelimiter +
													_response.Volume + _parameters.OutputDelimiter +
													_response.OpenInterest + _parameters.OutputDelimiter);
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
							_response.ParseMarketData(data);
							_response.ParseDateTime(data);

							writerToFile.WriteLine(_response.ResponseDate + _parameters.OutputDelimiter +
													_response.Open + _parameters.OutputDelimiter +
													_response.High + _parameters.OutputDelimiter +
													_response.Low + _parameters.OutputDelimiter +
													_response.Close + _parameters.OutputDelimiter +
													_response.Volume + _parameters.OutputDelimiter +
													_response.OpenInterest + _parameters.OutputDelimiter);
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
							_response.ParseMarketData(data);
							_response.ParseDateTime(data);

							writerToFile.WriteLine(_response.ResponseDate + _parameters.OutputDelimiter +
													_response.Open + _parameters.OutputDelimiter +
													_response.High + _parameters.OutputDelimiter +
													_response.Low + _parameters.OutputDelimiter +
													_response.Close + _parameters.OutputDelimiter +
													_response.Volume + _parameters.OutputDelimiter +
													_response.OpenInterest + _parameters.OutputDelimiter);
							break;
					}
				}
			}
		}

		#endregion
	}
}