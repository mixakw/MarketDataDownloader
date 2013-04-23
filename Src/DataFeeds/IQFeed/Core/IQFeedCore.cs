using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

using IQFeed.Models;

using log4net;

namespace IQFeed.Core
{
	public class IQFeedCore
	{
		private readonly ILog _logger;
		private readonly IQFeedProxy _proxy;
		private readonly IQFeedDataSaver _saver;

		private NetworkStream _network;

		public IQFeedCore(ILog logger, IQFeedProxy proxy, IQFeedDataSaver saver)
		{
			if (logger == null) throw new ArgumentNullException("logger");
			if (proxy == null) throw new ArgumentNullException("proxy");
			if (saver == null) throw new ArgumentNullException("saver");

			_logger = logger;
			_proxy = proxy;
			_saver = saver;
		}

		public void GetNetworkStream()
		{
			_network = _proxy.CreateNetworkStream();
		}

		public void SendRequest(string request)
		{
			var result = CreateSendRequest(request);
			_network.Write(result, 0, result.Length);
		}

		public void GetData(string request, string folder, IQFeedRequest input)
		{
			if (input == null) throw new ArgumentNullException("input");
			if (string.IsNullOrEmpty(request)) throw new ArgumentNullException("request");
			if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException("folder");
			
			using (var streamReader = new StreamReader(_network))
			{
				string line = streamReader.ReadLine();

				if (CheckContent(line))
				{
					int index = 0;
					int accumulationIndex = 0;
					List<string> data = new List<string>();

					while (!streamReader.EndOfStream)
					{
						line = streamReader.ReadLine();

						bool emptyLine = string.IsNullOrEmpty(line);
						bool endOfMessage = line.Contains(IQFeedConfiguration.EndMessage);

						if (!emptyLine && !endOfMessage)
						{
							data.Add(line);
							index++;

							if (index == 100000)
							{
								accumulationIndex = accumulationIndex + index;
								_logger.Info(string.Format("{0} entries of data processed", accumulationIndex));
								_saver.SaveData(input, folder, data);
								data.Clear();
								index = 0;
							}
						}

						_logger.Info(String.Format("{0} entries of data processed", index));
						_saver.SaveData(input, folder, data);
						data.Clear();
					}
				}
			}
		}

		private byte[] CreateSendRequest(string request)
		{
			return Encoding.UTF8.GetBytes(request + IQFeedConfiguration.EOL);
		}

		private bool CheckContent(string line)
		{
			bool isValidContent = true;

			if (!string.IsNullOrEmpty(line))
			{
				if (line.Contains(IQFeedConfiguration.NoDataError))
				{
					isValidContent = false;
					_logger.Error("No data");
				}

				if (line.Contains(IQFeedConfiguration.InvalidSymbolError))
				{
					isValidContent = false;
					_logger.Error("Invalid symbol");
				}

				if (line.Contains(IQFeedConfiguration.SyntaxError))
				{
					isValidContent = false;
					_logger.Error("Syntax error");
				}
			}

			return isValidContent;
		}
	}
}
