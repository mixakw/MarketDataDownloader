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
using MarketDataDownloader.DomainLogicLayer.Models;
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
            if (network.CanWrite)
            {
                network.Write(result, 0, result.Length);
                network.Flush();
            }
            else
            {
                _logger.Error("You cannon write in stream");
            }
            }

		public void GetData(string request, string folder, Parameters programParameters, IQFeedRequest input, NetworkStream network)
		{
            _saver.SetProgramParameters(programParameters);

      
            SendRequest("S,SET PROTOCOL,5.0", network);
        //    SendRequest("SBF,s,MSFT,t,2 9", network);

            SendRequest(request, network);
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
           
            using(StreamReader lstreamReader = new StreamReader(network))
           
            {

                var line = lstreamReader.ReadLine();
                Check50Protocol(line);
               
				ProcessData(folder, input, lstreamReader);
				
                lstreamReader.Close();
            }
		}


        private StreamWriter GetWriterToFile(string folder, string symbol)
        {
            var quotesfilepath = folder + @"\" + symbol +".txt";
            var fs = new FileStream(quotesfilepath, FileMode.Append, FileAccess.Write);
            return new StreamWriter(fs);
        }
        
        private void ProcessData(string folder, IQFeedRequest input, StreamReader streamReader)
		{
			var index = 0;
			var accumulationIndex = 0;

			var data = new List <string>();

            var line = streamReader.ReadLine();
             if (!CheckContent(line)) return;
             data.Add(line);
           //  _logger.Info("1");
             try
            {
                var streamWriter = GetWriterToFile(folder,input.CurrentSymbol);


                while (!streamReader.EndOfStream)
                {
                     line = streamReader.ReadLine();

                    if (!CheckEndMessage(line))
                    {
                        data.Add(line);
                        index++;

                        if (index == 100000)
                        {
                          
                            accumulationIndex = accumulationIndex + index;
                            _saver.SaveData(input, ref streamWriter, data);
                            
                            _logger.Info(string.Format("{0} entries of data processed", index));
                            data.Clear();
                            index = 0;
                        }
                    }
                    else
                    {
                        break;
                    }

                  
                }
               
                 _saver.SaveData(input, ref streamWriter, data);
                data.Clear();
                streamWriter.Close();
               // _logger.Info("2");
             }
            catch (UnauthorizedAccessException ex)
            {
               

            }
        }

		private bool CheckEndMessage(string line)
		{
			var isEnd = (line == null )|| string.IsNullOrEmpty(line) || line.Contains(IQFeedConfiguration.EndMessage);

			return isEnd;
		}
        
        const string CMDEND = "\r\n";

        public byte[] CreateSendRequest(params string[] commands)
		{
			//return Encoding.UTF8.GetBytes(request + IQFeedConfiguration.EOL);
            var cmddata = string.Join(CMDEND, commands) + CMDEND;
            byte[] data = new byte[cmddata.Length];
            data = Encoding.ASCII.GetBytes(cmddata);
            return data;
       
        }

        
        public  List<string> GetOptionsSymbol(string symbolanddescription, NetworkStream _network)
        {
         List<string> Symbols=new  List<string>();
          string []  tmpsplit = symbolanddescription.Split();
         string symbol=tmpsplit[0];
         string description = symbol;
         if (tmpsplit.Length > 1) description = tmpsplit[1];
       
           
         
            SendRequest("SBF,s,"+symbol+",t,2 9", _network);
         StreamReader lstreamReader = new StreamReader(_network);
            var line = lstreamReader.ReadLine();
         
            if (!CheckContent(line)) return Symbols;
            string OptionSymbol = (line.Split(new Char[] { ',' }))[0];
            string OptionDescription = (line.Split(new Char[] { ',' }))[3];
            string SymbolfromOptionDescription = OptionDescription.Split()[0];
            if (OptionSymbol.IndexOf(symbol) == 0 && description == SymbolfromOptionDescription)
            {
                Symbols.Add(OptionSymbol);
            }
            while (!lstreamReader.EndOfStream)
                {
                     line = lstreamReader.ReadLine();
                   
                    if (!CheckEndMessage(line))
                     {
                         tmpsplit = line.Split(new Char[] { ',' });
                         OptionSymbol = tmpsplit[0];
                         OptionDescription = tmpsplit[3];
                         SymbolfromOptionDescription = OptionDescription.Split()[0];  
                        if (OptionSymbol.IndexOf(symbol) == 0 && description == SymbolfromOptionDescription)
                         {
                            Symbols.Add(OptionSymbol);
                        }
                     }
                     else
                     {
                         break;
                     }
                     }

        lstreamReader.Close();
        return Symbols;
        }
        
                    
                    private void Check50Protocol(string line)
        {
            
            if (!string.IsNullOrEmpty(line))
            {
                if (line.Contains(IQFeedConfiguration.Current50Protocol))
                {


                    _logger.Info("Current 5.0 protocol");
                }
                else
                {
                    _logger.Warn("Current not 5.0 protocol. Install IQFeed 5.0.");
                }

            }
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
			
            	if (line.Contains(IQFeedConfiguration.CantConnectHistorySocket))
				{
					isValidContent = false;
                    _logger.Error("Could not connect to History socket");
				}
            
                if (line.Contains(IQFeedConfiguration.CantConnectSymbolLookupSocket))
				{
					isValidContent = false;
                    _logger.Error("Could not connect to SymbolLookup socket.");
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
