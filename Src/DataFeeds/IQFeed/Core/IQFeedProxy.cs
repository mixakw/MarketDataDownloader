#region FileHeader

// File:
// MarketDataDownloader/IQFeed/IQFeedProxy.cs
// 
// Last updated:
// 2013-05-23 11:29 AM

#endregion

#region Usings

using System;
using System.Net.Sockets;
using MarketDataDownloader.DomainLogicLayer.Abstraction;
using MarketDataDownloader.Logging;

#endregion

namespace IQFeed.Core
{
	public class IQFeedProxy : IDataFeedProxy
	{
		private readonly IMyLogger _logger;

		public IQFeedProxy(IMyLogger logger)
		{
			if (logger == null) throw new ArgumentNullException("logger");

			_logger = logger;
		}

		public TcpClient Connect()
		{
			var socket = new TcpClient();

			try
			{
				socket = new TcpClient(IQFeedConfiguration.IQFeedHostName, IQFeedConfiguration.IQFeedHistoryPort);
			}
			catch (SocketException ex)
			{
				_logger.Error(ex.Message);
			}

			return socket;
		}

        public TcpClient ConnectToLevel1()
        {
            var socket = new TcpClient();

            try
            {
                socket = new TcpClient(IQFeedConfiguration.IQFeedHostName, IQFeedConfiguration.IQFeedLevel1Port);
            }
            catch (SocketException ex)
            {
                _logger.Error(ex.Message);
            }

            return socket;
        }
		
        public NetworkStream CreateNetworkStream(TcpClient socket)
		{
			if (socket == null) throw new ArgumentNullException("socket");
			if (!socket.Connected) throw new ArgumentNullException("socket");

			var networkStream = socket.GetStream();

			if (!networkStream.CanRead && !networkStream.CanWrite)
			{
				_logger.Error("You cannot read/write data from this stream.");
				networkStream.Close();
			}

			return networkStream;
		}
	}
}