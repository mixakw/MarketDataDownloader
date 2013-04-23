namespace MarketDataDownloader.DomainLogicLayer.Models
{
	public class Parameters
	{
		public string OutputDelimiter { get; set; }
		public string OutputTerminater { get; set; }

		public string DateFormat { get; set; }
		public string TimeFormat { get; set; }
		public string DateTimeDelimeter { get; set; }

		public bool IsRealTime { get; set; }
	}
}
