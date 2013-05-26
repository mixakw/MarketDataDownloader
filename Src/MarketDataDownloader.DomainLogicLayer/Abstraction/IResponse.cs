namespace MarketDataDownloader.DomainLogicLayer.Abstraction
{
	public interface IResponse
	{
		string RequestId { get; set; }
		string High { get; set; }
		string Low { get; set; }
		string Open { get; set; }
		string Close { get; set; }
		string Volume { get; set; }
		string OpenInterest { get; set; }
		string Last { get; set; }
		string LastSize { get; set; }
		string TotalVolume { get; set; }
		string Bid { get; set; }
		string Ask { get; set; }
		string TickId { get; set; }
		string BidSize { get; set; }
		string AskSize { get; set; }
		string TradeType { get; set; }
	}
}