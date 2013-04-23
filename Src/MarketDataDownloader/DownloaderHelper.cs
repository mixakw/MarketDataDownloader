using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

using log4net;

namespace MarketDataDownloader
{
	public class DownloaderHelper
	{
		#region Constants

		private const string NodataError = "2500,E,!NO_DATA!,";
		private const string EndMessage = "2500,!ENDMSG!,";
		private const string SyntaxError = "2500,E,!SYNTAX_ERROR!,";
		private const string InvalidSymbolError = "2500,E,Invalid symbol.,";
		private const string EndSymbol = "!";

		private const int SecondsPerMinute = 60;

		private const string IqfeedHostname = "127.0.0.1";
		private const int IqfeedHistoryPort = 9100;

		private const string DateSeparator = "-";
		private const string TimeSeparator = " ";
		private static readonly object TerminatingCharacter = Environment.NewLine;

		private const string DelimitingCharacter = ",";

		private const string TxtExstension = ".txt";
		private const int NumberOfIterations = 100000;
		private const byte EOL = 0x0;

		#endregion

		#region Variables

		private static ILog _logger = LogManager.GetLogger(typeof(DownloaderHelper));

		#endregion

		#region Public Methods

		public TcpClient OpenTcpClientConnection()
		{
			TcpClient socket = null;

			try
			{
				socket = new TcpClient(IqfeedHostname, IqfeedHistoryPort);
			}
			catch (SocketException)
			{

			}

			return socket;
		}

		public NetworkStream CreateStream(TcpClient socket)
		{
			if (socket == null) throw new ArgumentNullException("socket");

			NetworkStream netStream = socket.GetStream();
			return netStream;
		}

		public string CreateRequest(string timeframeName, int timeframeValue, string symbol, int days, string beginDateTime, string endDateTime)
		{
			if (timeframeName == null) throw new ArgumentNullException("timeframeName");
			if (symbol == null) throw new ArgumentNullException("symbol");
			if (beginDateTime == null) throw new ArgumentNullException("beginDateTime");
			if (endDateTime == null) throw new ArgumentNullException("endDateTime");

			int datapoints = 0;
			string beginFilterTime = "000000";
			string endFilterTime = "235959";
			int datapointsPerSend = 2500;
			int direction = 1;

			switch (timeframeName)
			{
				case "Tick Days":
					#region Comment
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
					#endregion
					return "HTD" + DelimitingCharacter + symbol + DelimitingCharacter + days + DelimitingCharacter + datapoints + DelimitingCharacter +
						beginFilterTime + DelimitingCharacter + endFilterTime + DelimitingCharacter +
						direction + DelimitingCharacter + datapointsPerSend + TerminatingCharacter;

				case "Tick Interval":
					#region Comment
					//HTT,[Symbol],[BeginDate BeginTime],[EndDate EndTime],[MaxDatapoints],[BeginFilterTime],[EndFilterTime],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves tick data between [BeginDate BeginTime] and [EndDate EndTime] for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[BeginDate BeginTime] - Required if [EndDate EndTime] not specified - Format CCYYMMDD HHmmSS - Earliest date/time to receive data for.
					//[EndDate EndTime] - Required if [BeginDate BeginTime] not specified - Format CCYYMMDD HHmmSS - Most recent date/time to receive data for.
					//[MaxDatapoints] - Optional - the maximum number of datapoints to be retrieved.
					//[BeginFilterTime] - Optional - Format HHmmSS - Allows you to specify the earliest time of day (EST) for which to receive data.
					//[EndFilterTime] - Optional - Format HHmmSS - Allows you to specify the latest time of day (EST) for which to receive data.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of datapoints that IQConnect.exe will queue before attempting to send across the socket to your app. 
					#endregion
					return "HTT" + DelimitingCharacter + symbol + DelimitingCharacter + ConvertDateTime(beginDateTime, false) + DelimitingCharacter +
						ConvertDateTime(endDateTime, false) + DelimitingCharacter + datapoints + DelimitingCharacter + beginFilterTime +
						DelimitingCharacter + endFilterTime + DelimitingCharacter + direction + DelimitingCharacter + datapointsPerSend + TerminatingCharacter;

				case "Intraday Days":
					#region Comment
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
					#endregion
					return "HID" + DelimitingCharacter + symbol + DelimitingCharacter + timeframeValue * SecondsPerMinute + DelimitingCharacter +
						days + DelimitingCharacter + datapoints + DelimitingCharacter + beginFilterTime + DelimitingCharacter + endFilterTime +
						DelimitingCharacter + direction + DelimitingCharacter + datapointsPerSend + TerminatingCharacter;

				case "Intraday Interval":
					#region Comment
					//HIT,[Symbol],[Interval],[BeginDate BeginTime],[EndDate EndTime],[MaxDatapoints],[BeginFilterTime],[EndFilterTime],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves interval data between [BeginDate BeginTime] and [EndDate EndTime] for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[Interval] - Required - The interval in seconds.
					//[BeginDate BeginTime] - Required if [EndDate EndTime] not specified - Format CCYYMMDD HHmmSS - Earliest date/time to receive data for.
					//[EndDate EndTime] - Required if [BeginDate BeginTime] not specified - Format CCYYMMDD HHmmSS - Most recent date/time to receive data for.
					//[MaxDatapoints] - Optional - the maximum number of datapoints to be retrieved.
					//[BeginFilterTime] - Optional - Format HHmmSS - Allows you to specify the earliest time of day (EST) for which to receive data.
					//[EndFilterTime] - Optional - Format HHmmSS - Allows you to specify the latest time of day (EST) for which to receive data.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of datapoints that IQConnect.exe will queue before attempting to send across the socket to your app. 
					#endregion
					return "HIT" + DelimitingCharacter + symbol + DelimitingCharacter + timeframeValue * SecondsPerMinute + DelimitingCharacter +
						ConvertDateTime(beginDateTime, false) + DelimitingCharacter + ConvertDateTime(endDateTime, false) + DelimitingCharacter +
						datapoints + DelimitingCharacter + beginFilterTime + DelimitingCharacter + endFilterTime + DelimitingCharacter +
						direction + DelimitingCharacter + datapointsPerSend + TerminatingCharacter;

				case "Daily Days":
					#region Comment
					//HDX,[Symbol],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves up to [MaxDatapoints] number of End-Of-Day Data for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[MaxDatapoints] - Required - The maximum number of datapoints to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of datapoints that IQConnect.exe will queue before attempting to send across the socket to your app. 
					#endregion
					return "HDX" + DelimitingCharacter + symbol + DelimitingCharacter + datapoints + DelimitingCharacter + direction +
						DelimitingCharacter + datapointsPerSend + TerminatingCharacter;

				case "Daily Interval":
					#region Comment
					//HDT,[Symbol],[BeginDate],[EndDate],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves Daily data between [BeginDate] and [EndDate] for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[BeginDate] - Required if [EndDate] not specified - Format CCYYMMDD - Earliest date to receive data for.
					//[EndDate] - Required if [BeginDate] not specified - Format CCYYMMDD - Most recent date to receive data for.
					//[MaxDatapoints] - Optional - the maximum number of datapoints to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of datapoints that IQConnect.exe will queue before attempting to send across the socket to your app. 
					#endregion
					return "HDT" + DelimitingCharacter + symbol + DelimitingCharacter + ConvertDateTime(beginDateTime, false) + DelimitingCharacter +
						ConvertDateTime(endDateTime, false) + DelimitingCharacter + datapoints + DelimitingCharacter + direction +
						DelimitingCharacter + datapointsPerSend + TerminatingCharacter;

				case "Weekly Days":
					#region Comment
					//HWX,[Symbol],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves up to [MaxDatapoints] data points of composite weekly data points for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[MaxDatapoints] - Required - The maximum number of data points to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of data points that IQConnect.exe will queue before attempting to send across the socket to your app. 
					#endregion
					return "HWX" + DelimitingCharacter + symbol + DelimitingCharacter + datapoints + DelimitingCharacter + direction +
						DelimitingCharacter + datapointsPerSend + TerminatingCharacter;

				case "Monthly Days":
					#region Comment
					//HMX,[Symbol],[MaxDatapoints],[DataDirection],[RequestID],[DatapointsPerSend]<CR><LF> 
					//Retrieves up to [MaxDatapoints] datapoints of composite monthly datapoints for the specified [Symbol].
					//[Symbol] - Required - Max Length 30 characters.
					//[MaxDatapoints] - Required - The maximum number of datapoints to be retrieved.
					//[DataDirection] - Optional - '0' (default) for "newest to oldest" or '1' for "oldest to newest".
					//[RequestID] - Optional - Will be sent back at the start of each line of data returned for this request.
					//[DatapointsPerSend] - Optional - Specifies the number of datapoints that IQConnect.exe will queue before attempting to send across the socket to your app. 
					#endregion
					return "HMX" + DelimitingCharacter + symbol + DelimitingCharacter + datapoints + DelimitingCharacter + direction +
						DelimitingCharacter + datapointsPerSend + TerminatingCharacter;

				default:
					_logger.Error("Request error");
					return string.Empty;
			}
		}

		public void GetData(NetworkStream netStream, string request, string folder, string timeframeName, string symbol)
		{
			if (netStream == null) throw new ArgumentNullException("netStream");
			if (request == null) throw new ArgumentNullException("request");
			if (folder == null) throw new ArgumentNullException("folder");
			if (timeframeName == null) throw new ArgumentNullException("timeframeName");
			if (symbol == null) throw new ArgumentNullException("symbol");

			if (!CheckStream(netStream)) return;

			SendRequest(request, netStream);

			var downloadedData = new List<string>();
			StreamReader netStreamReader = new StreamReader(netStream);
			StreamWriter writerToFile;

			if (!IsValidContent(netStreamReader)) return;

			int index = 0;
			int accumulationIndex = 0;
			string line;

			while ((line = netStreamReader.ReadLine()) != EndMessage)
			{
				downloadedData.Add(line);
				index++;

				if (index == NumberOfIterations)
				{
					accumulationIndex = accumulationIndex + index;

					_logger.Info(String.Format("{0} entries of data processed", accumulationIndex));

					writerToFile = GetWriterToFile(folder, symbol);
					ParseData(timeframeName, downloadedData, writerToFile);
					writerToFile.Close();

					downloadedData.Clear();
					index = 0;
				}
			}

			if (index == 0) return;

			_logger.Info(String.Format("{0} entries of data processed", index));

			writerToFile = GetWriterToFile(folder, symbol);
			ParseData(timeframeName, downloadedData, writerToFile);
			writerToFile.Close();

			downloadedData.Clear();
		}

		#endregion

		#region Private Methods

		private bool CheckStream(NetworkStream netStream)
		{
			bool isSuccessful = true;

			if (!netStream.CanRead)
			{
				_logger.Error("You cannot read data from this stream.");
				netStream.Close();
				isSuccessful = false;
			}

			if (!netStream.CanWrite)
			{
				_logger.Error("You cannot write data to this stream.");
				netStream.Close();
				isSuccessful = false;
			}
			return isSuccessful;
		}

		private void SendRequest(string request, NetworkStream netStream)
		{
			var sendBytes = Encoding.UTF8.GetBytes(request + EOL);
			netStream.Write(sendBytes, 0, sendBytes.Length);
		}

		private StreamWriter GetWriterToFile(string folder, string symbol)
		{
			var quotesfilepath = folder + @"\" + symbol + TxtExstension;
			var fs = new FileStream(quotesfilepath, FileMode.Append, FileAccess.Write);
			return new StreamWriter(fs);
		}

		private bool IsValidContent(StreamReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			bool isValidContent = true;

			string line = reader.ReadLine();

			if (line == NodataError)
			{
				isValidContent = false;
				_logger.Error("No data");
			}

			if (line == InvalidSymbolError)
			{
				isValidContent = false;
				_logger.Error("Invalid symbol");
			}

			if (line == SyntaxError)
			{
				isValidContent = false;
				_logger.Error("Syntax error");
			}

			return isValidContent;
		}

		private void ParseData(string timeframeName, IList<string> dataArray, StreamWriter writerToFile)
		{
			if (dataArray == null) throw new ArgumentNullException("dataArray");
			if (writerToFile == null) throw new ArgumentNullException("writerToFile");
			
			if (dataArray.Count == 0) return;
			if (dataArray[0].Substring(0, 1) == EndSymbol) return;

			foreach (var data in dataArray)
			{
				var dataArraySplit = data.Split(',');
				var entities = new DownloaderEntities();

				switch (timeframeName)
				{
					#region Comment
					//"HTD"
					//Request ID. Text.
					//Time Stamp. Text. Example: 2008-09-01 16:00:01
					//Last. Decimal. Example: 146.2587
					//Last Size. Integer. Example: 100
					//Total Volume.	Integer. Example: 1285001
					//Bid. Decimal. Example: 146.2400
					//Ask. Decimal.	Example: 146.2600
					//TickID. Integer. Example: 6813524
					//Bid Size. Integer. Example: 100
					//Ask Size. Integer. Example: 100
					//Basis For Last. Character. Current Possible values are 'C' (normal trade) or 'E' (extended trade). 
					#endregion
					case "Tick Days":
						entities.ParseData(dataArraySplit, true);
						writerToFile.WriteLine(entities.TickId + DelimitingCharacter + entities.TradeType + DelimitingCharacter + entities.Year + DateSeparator + entities.Month + DateSeparator + entities.Day + TimeSeparator + entities.Time +
						",{0},{1},{2},{3},{4},{5}", entities.LastAsString(), entities.LastSize, entities.BidAsString(), entities.AskAsString(), entities.BidSize, entities.AskSize);
						break;

					#region Comment
					//"HTT"
					//Request ID. Text.
					//Time Stamp. Text. Example: 2008-09-01 16:00:01
					//Last. Decimal. Example: 146.2587
					//Last Size. Integer. Example: 100
					//Total Volume.	Integer. Example: 1285001
					//Bid. Decimal. Example: 146.2400
					//Ask. Decimal.	Example: 146.2600
					//TickID. Integer. Example: 6813524
					//Bid Size. Integer. Example: 100
					//Ask Size. Integer. Example: 100
					//Basis For Last. Character. Current Possible values are 'C' (normal trade) or 'E' (extended trade).
					#endregion
					case "Tick Interval":
						entities.ParseData(dataArraySplit, true);
						writerToFile.WriteLine(entities.TickId + DelimitingCharacter + entities.TradeType + DelimitingCharacter + entities.Year + DateSeparator + entities.Month + DateSeparator + entities.Day + TimeSeparator + entities.Time +
						",{0},{1},{2},{3},{4},{5}", entities.LastAsString(), entities.LastSize, entities.BidAsString(), entities.AskAsString(), entities.BidSize, entities.AskSize);
						break;

					#region Comment
					//"HID"
					//Request ID. Text.
					//Time Stamp. CCYY-MM-DD HH:MM:SS	Example: 2008-09-01 16:00:01
					//High. Decimal. Example: 146.2587
					//Low. Decimal. Example: 145.2587
					//Open.	Decimal. Example: 146.2587
					//Close. Decimal. Example: 145.2587
					//Total Volume.	Integer. Example: 1285001
					//Period Volume. Integer. Example: 1285
					#endregion
					case "Intraday Days":
						entities.ParseData(dataArraySplit, false);
						writerToFile.WriteLine(entities.Year + DateSeparator + entities.Month + DateSeparator + entities.Day + TimeSeparator + entities.Time +
						",{0},{1},{2},{3},{4}", entities.OpenAsString(), entities.HighAsString(), entities.LowAsString(), entities.CloseAsString(), entities.Volume);
						break;

					#region Comment
					//"HIT"
					//Request ID. Text.
					//Time Stamp. CCYY-MM-DD HH:MM:SS	Example: 2008-09-01 16:00:01
					//High. Decimal. Example: 146.2587
					//Low. Decimal. Example: 145.2587
					//Open.	Decimal. Example: 146.2587
					//Close. Decimal. Example: 145.2587
					//Total Volume.	Integer. Example: 1285001
					//Period Volume. Integer. Example: 1285
					#endregion
					case "Intraday Interval":
						entities.ParseData(dataArraySplit, false);
						writerToFile.WriteLine(entities.Year + DateSeparator + entities.Month + DateSeparator + entities.Day + TimeSeparator + entities.Time +
						",{0},{1},{2},{3},{4}", entities.OpenAsString(), entities.HighAsString(), entities.LowAsString(), entities.CloseAsString(), entities.Volume);
						break;

					#region Comment
					//"HDX"
					//Request ID. Text.
					//Time Stamp. CCYY-MM-DD HH:MM:SS. Example: 2008-09-01 16:00:01
					//High. Decimal. Example: 146.2587
					//Low. Decimal. Example: 145.2587
					//Open. Decimal. Example: 146.2587
					//Close. Decimal. Example: 145.2587
					//Period Volume. Integer. Example: 1285001
					//Open Interest. Integer. Example: 128
					#endregion
					case "Daily Days":
						entities.ParseData(dataArraySplit, false);
						writerToFile.WriteLine(entities.Year + DateSeparator + entities.Month + DateSeparator + entities.Day + ",{0},{1},{2},{3},{4},{5}",
						entities.OpenAsString(), entities.HighAsString(), entities.LowAsString(), entities.CloseAsString(), entities.Volume, entities.OpenInterest);
						break;

					#region Comment
					//"HDT"
					//Request ID. Text.
					//Time Stamp. CCYY-MM-DD HH:MM:SS. Example: 2008-09-01 16:00:01
					//High. Decimal. Example: 146.2587
					//Low. Decimal. Example: 145.2587
					//Open. Decimal. Example: 146.2587
					//Close. Decimal. Example: 145.2587
					//Period Volume. Integer. Example: 1285001
					//Open Interest. Integer. Example: 128
					#endregion
					case "Daily Interval":
						entities.ParseData(dataArraySplit, false);
						writerToFile.WriteLine(entities.Year + DateSeparator + entities.Month + DateSeparator + entities.Day + ",{0},{1},{2},{3},{4},{5}",
						entities.OpenAsString(), entities.HighAsString(), entities.LowAsString(), entities.CloseAsString(), entities.Volume, entities.OpenInterest);
						break;

					#region Comment
					//"HWX"
					//Request ID. Text.
					//Time Stamp. CCYY-MM-DD HH:MM:SS. Example: 2008-09-01 16:00:01
					//High. Decimal. Example: 146.2587
					//Low. Decimal. Example: 145.2587
					//Open. Decimal. Example: 146.2587
					//Close. Decimal. Example: 145.2587
					//Period Volume. Integer. Example: 1285001
					//Open Interest. Integer. Example: 128
					#endregion
					case "Weekly Days":
						entities.ParseData(dataArraySplit, false);
						writerToFile.WriteLine(entities.Year + DateSeparator + entities.Month + DateSeparator + entities.Day + ",{0},{1},{2},{3},{4},{5}",
						entities.OpenAsString(), entities.HighAsString(), entities.LowAsString(), entities.CloseAsString(), entities.Volume, entities.OpenInterest);
						break;

					#region Comment
					//"HMX"
					//Request ID. Text.
					//Time Stamp. CCYY-MM-DD HH:MM:SS. Example: 2008-09-01 16:00:01
					//High. Decimal. Example: 146.2587
					//Low. Decimal. Example: 145.2587
					//Open. Decimal. Example: 146.2587
					//Close. Decimal. Example: 145.2587
					//Period Volume. Integer. Example: 1285001
					//Open Interest. Integer. Example: 128
					#endregion
					case "Monthly Days":
						entities.ParseData(dataArraySplit, false);
						writerToFile.WriteLine(entities.Year + DateSeparator + entities.Month + DateSeparator + entities.Day + ",{0},{1},{2},{3},{4},{5}",
						entities.OpenAsString(), entities.HighAsString(), entities.LowAsString(), entities.CloseAsString(), entities.Volume, entities.OpenInterest);
						break;
				}
			}
		}

		private string ConvertDateTime(string input, bool isCustomInterval)
		{
			var dateTime = new StringBuilder();

			string day = input.Substring(0, 2);
			string month = input.Substring(3, 2);
			string year = input.Substring(6, 4);

			string hour = "00";
			string minute = "00";
			string second = "00";

			// Not used
			if (isCustomInterval)
			{
				hour = input.Substring(11, 2);
				minute = input.Substring(14, 2);
				second = input.Substring(17, 2);
			}

			dateTime.Append(year);
			dateTime.Append(month);
			dateTime.Append(day);
			dateTime.Append(' ');
			dateTime.Append(hour);
			dateTime.Append(minute);
			dateTime.Append(second);

			return dateTime.ToString();
		}

		#endregion
	}
}