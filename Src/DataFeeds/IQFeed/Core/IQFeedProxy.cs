using System;
using System.Net.Sockets;

using log4net;

namespace IQFeed.Core
{
	public class IQFeedProxy
	{
		private TcpClient _socket;
		private readonly ILog _logger;

		public IQFeedProxy(ILog logger)
		{
			if (logger == null) throw new ArgumentNullException("logger");

			_logger = logger;
		}

		public TcpClient Connect()
		{
			try
			{
				_socket = new TcpClient(IQFeedConfiguration.IQFeedHostName, IQFeedConfiguration.IQFeedHistoryPort);
			}
			catch (SocketException ex)
			{
				_logger.Error(ex.Message);
			}

			return _socket;
		}

		public NetworkStream CreateNetworkStream()
		{
			if (_socket == null)
			{
				Connect();
			}

			NetworkStream networkStream = _socket.GetStream();

			if (!networkStream.CanRead && !networkStream.CanWrite)
			{
				_logger.Error("You cannot read/write data from this stream.");
				networkStream.Close();
			}

			return networkStream;
		}
	}
}
