// =================================================
// File:
// MarketDataDownloader/IQFeed/IQFeedCore.cs
// 
// Last updated:
// 2013-05-24 4:40 PM
// =================================================

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

using IQFeed.Models;

using MarketDataDownloader.DomainLogicLayer.Abstraction;
using MarketDataDownloader.Logging;

#endregion

namespace IQFeed.Core
{
	public class IQFeedCore : IDataFeedCore
	{
		private readonly IMyLogger _logger;
		private readonly IQFeedDataSaver _saver;

		public IQFeedCore(IMyLogger logger, IQFeedDataSaver saver)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			if (saver == null)
			{
				throw new ArgumentNullException("saver");
			}

			_logger = logger;
			_saver = saver;
		}

		public void SendRequest(string request, NetworkStream network)
		{
			var result = CreateSendRequest(request);
			network.Write(result, 0, result.Length);
		}

		public void GetData(string request, string folder, IQFeedRequest input, NetworkStream network)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (string.IsNullOrEmpty(request))
			{
				throw new ArgumentNullException("request");
			}
			if (string.IsNullOrEmpty(folder))
			{
				throw new ArgumentNullException("folder");
			}

			using (var streamReader = new StreamReader(network))
			{
				var line = streamReader.ReadLine();

				if (CheckContent(line))
				{
					ProcessData(folder, input, streamReader);
				}
			}
		}

		private void ProcessData(string folder, IQFeedRequest input, StreamReader streamReader)
		{
			var index = 0;
			var accumulationIndex = 0;

			var data = new List <string>();
			var streamWriter = new StreamWriter(folder);

			while (!streamReader.EndOfStream)
			{
				var line = streamReader.ReadLine();

				if (CheckEndMessage(line))
				{
					data.Add(line);
					index++;

					if (index == 100000)
					{
						accumulationIndex = accumulationIndex + index;
						_saver.SaveData(input, streamWriter, data);
						_logger.Info(string.Format("{0} entries of data processed", index));
						data.Clear();
						index = 0;
					}
				}

				_saver.SaveData(input, streamWriter, data);
				data.Clear();
			}
		}

		private bool CheckEndMessage(string line)
		{
			var isEnd = line == null || string.IsNullOrEmpty(line) || line.Contains(IQFeedConfiguration.EndMessage);

			return isEnd;
		}

		private byte[] CreateSendRequest(string request)
		{
			return Encoding.UTF8.GetBytes(request + IQFeedConfiguration.EOL);
		}

		private bool CheckContent(string line)
		{
			var isValidContent = true;

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

		public void Test()
		{
			_logger.Error("Core");
		}
	}
}
