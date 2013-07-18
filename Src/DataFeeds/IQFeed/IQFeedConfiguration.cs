#region FileHeader

// File:
// MarketDataDownloader/IQFeed/IQFeedConfiguration.cs
// 
// Last updated:
// 2013-05-22 7:02 PM

#endregion

#region Usings

using System;

#endregion

namespace IQFeed
{
	public static class IQFeedConfiguration
	{
		public static string NoDataError = "NO_DATA";
		public static string EndMessage = "ENDMSG";
		public static string SyntaxError = "SYNTAX_ERROR";
        public static string Current50Protocol = "S,CURRENT PROTOCOL,5.0";
        public static string InvalidSymbolError = "Invalid symbol";
        public static string CantConnectHistorySocket = "Could not connect to History socket";
        public static string CantConnectSymbolLookupSocket = "Could not connect to SymbolLookup socket";
        public static string IQFeedHostName = "127.0.0.1";
		public static int IQFeedHistoryPort = 9100;
        public static int IQFeedLevel1Port = 9100;


		public static byte EOL = 0x0;

		public static string Delimiter = ",";
		public static string Terminater = Environment.NewLine;

		public static string TickDaysHeader = "HTD";
		public static string TickIntervalHeader = "HTT";
		public static string IntradayDaysHeader = "HID";
		public static string IntradayIntervalHeader = "HIT";
		public static string DailyDaysHeader = "HDX";
		public static string DailyIntervalHeader = "HDT";
		public static string WeeklyDaysHeader = "HWX";
		public static string MonthlyDaysHeader = "HMX";
	}
}