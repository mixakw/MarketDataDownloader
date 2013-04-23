using MarketDataDownloader.DomainLogicLayer.Abstraction;

namespace IQFeed.Models
{
	public class IQFeedResponse : BaseResponse
	{
		public string RequestId { get; set; }
		public string High { get; set; }
		public string Low { get; set; }
		public string Open { get; set; }
		public string Close { get; set; }
		public string Volume { get; set; }
		public string OpenInterest { get; set; }
		public string Last { get; set; }
		public string LastSize { get; set; }
		public string TotalVolume { get; set; }
		public string Bid { get; set; }
		public string Ask { get; set; }
		public string TickId { get; set; }
		public string BidSize { get; set; }
		public string AskSize { get; set; }
		public string TradeType { get; set; }
	}
}
