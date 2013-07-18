// =================================================
// File:
// MarketDataDownloader/MarketDataDownloader.UI/MainForm.cs
// 
// Last updated:
// 2013-05-24 3:58 PM
// =================================================

#region Usings

using System;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using IQFeed.Core;
using IQFeed.Models;

using MarketDataDownloader.DI;
using MarketDataDownloader.DomainLogicLayer.Abstraction;
using MarketDataDownloader.DomainLogicLayer.Helpers;
using MarketDataDownloader.DomainLogicLayer.Models;
using MarketDataDownloader.Logging;
using MarketDataDownloader.UI.Properties;

using Microsoft.Practices.Unity;
using System.Security.AccessControl;
#endregion

namespace MarketDataDownloader.UI
{
	public partial class MainForm : Form
	{
		private TcpClient _client;
		
        private IQFeedCore _core;

		private CancellationTokenSource _cts;
		private MyLogger _logger;
		private NetworkStream _network;
        private NetworkStream _networkLevel1;
        private IProgress<KeyValuePair<string, string>> _progress;
		private IQFeedProxy _proxy;
		private IQFeedQueryBuilder _queryBuilder;

		public MainForm()
		{
			InitializeComponent();

			InitVariables();
			ConnectToDataFeed();
            SetDefaultValues();
			InitAsyncRelated();
		 
        }

		private void InitVariables()
		{
			DependencyFactory.Container.RegisterInstance(new RichTextBoxAppender(rtbLog));

			_logger =
				(MyLogger)DependencyFactory.Container.Resolve<IMyLogger>(new ParameterOverride("currentClassName", "MarketDataDownloader.UI.MainForm"));

			_core = (IQFeedCore)DependencyFactory.Container.Resolve<IDataFeedCore>();
			_proxy = (IQFeedProxy)DependencyFactory.Container.Resolve<IDataFeedProxy>();
			_queryBuilder = (IQFeedQueryBuilder)DependencyFactory.Container.Resolve<IDataFeedQueryBuilder>();
		}

		private Parameters LoadProgramParameters()
		{
			var parameters = new Parameters();

			parameters.DateFormat = cbDate.Text;
			parameters.DateTimeDelimeter = cbDateTimeSeparator.Text;
			parameters.TimeFormat = cbTime.Text;
			parameters.OutputDelimiter = FieldDelimiter.Text;

			return parameters;
		}

		private IRequest LoadRequestParameters()
		{
			IRequest request = new IQFeedRequest();

			request.Symbols = GetTickersList();
			request.CurrentSymbol = String.Empty;

			if (rbDays.Checked)
			{
				request.TimeFrameType = "Days";
			}

			if (rbInterval.Checked)
			{
				request.TimeFrameType = "Interval";
			}

			request.TimeFrameName = cbTimeframe.SelectedItem.ToString();

            request.BeginDate = dtpBeginDate.Value.ToString("yyyyMMdd");
            request.EndDate = dtpEndDate.Value.ToString("yyyyMMdd");
            request.BeginTime = dtpBeginTime.Value.ToString("HHmmss");
            request.EndTime = dtpEndTime.Value.ToString("HHmmss");
            
            
			request.Days = tbAmountOfDays.Text;

			int interval;
			Int32.TryParse(cbTimeframeIntraday.SelectedItem.ToString(), out interval);
			request.Interval = interval;

			request.BeginFilterTime = "";
			request.EndFilterTime = "";

			

			request.DataDirection = "1";
			request.DatapointsPerSend = "";
			request.MaxDatapoints = "";
			request.RequestID = "";

			return request;
		}

      


     

        private async void BtnStartClick(object sender, EventArgs e)
		{
            try
            {
                LoadRequestParameters();
                LockControls();

                if (CheckSavePath() && CheckConnection())
                {
                    var programParameters = LoadProgramParameters();
                    var requestParameters = LoadRequestParameters();
          
                    var result = await DownloadDataAsync(programParameters, requestParameters);
           
                    _progress.Report(new KeyValuePair<string, string>("INFO", result));
                }

                UnlockControls();
         
                _logger.Info("Done");
            }
            catch (Exception ex)
            {
                _logger.Info("BtnStartClick_Exception");
                _logger.Info(ex.ToString());
            }
        }

		private void InitAsyncRelated()
		{
			_cts = new CancellationTokenSource();
			_progress = new Progress<KeyValuePair<string, string>>(ReportProgress);
		}

        private string DownloadData(Parameters programParameters, IRequest requestParameters)
        {
            
                foreach (var symbol in requestParameters.Symbols)
                {

                    List<string> Symbols=new  List<string>();
                    if (chbOptions.Checked)
                    {
                        Symbols = _core.GetOptionsSymbol(symbol, _network);
                    }
                    else
                    {
                        Symbols.Add(symbol);
                    }

                    int count = 0;

                    foreach (var lsymbol in Symbols)
                    {
                        requestParameters.CurrentSymbol = lsymbol;
                        if (chbOptions.Checked)
                        {
                            count += 1;
                         //   _progress.Report(new KeyValuePair<string, string>("INFO", String.Format("Option Symbol {0} from #{1}", count, Symbols.Count)));
                            _logger.Info("INFO:"+ String.Format("Option Symbol {0} from #{1}", count, Symbols.Count));
                        }

                        if (_cts.IsCancellationRequested)
                        {
                         //   _progress.Report(new KeyValuePair<string, string>("INFO", "Stopping proccess..."));
                            _logger.Info("INFO:Stopping proccess...");
                            break;
                        }
                        else
                        {
                      
                          //  _progress.Report(new KeyValuePair<string, string>("INFO", String.Format("Symbol {0}", requestParameters.CurrentSymbol)));
                            _logger.Info("INFO:" + String.Format("Symbol {0}", requestParameters.CurrentSymbol));

                            UploadAndProcess(programParameters, requestParameters);
                        }

                    }
                }


                return "Done";
        
        }
        
         
        
        
        private async Task<string> DownloadDataAsync(Parameters programParameters, IRequest requestParameters)
        {
            return await Task.Run<string>(() => DownloadData(programParameters, requestParameters));
           //return  DownloadData(programParameters, requestParameters);
          
        }

		public  void UploadAndProcess(Parameters programParameters, IRequest requestParameters)
		{


            if (chbBeginDate.Checked)
            {
                DateTime LastDateTime = GetLastDateFromFile(tbFolder.Text, requestParameters.CurrentSymbol, programParameters);
                if (LastDateTime !=default(DateTime))
                {
                   LastDateTime= LastDateTime.AddSeconds(1);
                    requestParameters.TimeFrameType = "Interval";
                    requestParameters.BeginDate = LastDateTime.ToString("yyyyMMdd");
                    requestParameters.EndDate = dtpEndDate.Value.ToString("yyyyMMdd");
                    requestParameters.BeginTime = LastDateTime.ToString("HHmmss");
                    requestParameters.EndTime = dtpEndTime.Value.ToString("HHmmss");
                  
                }
                }
      
            var query = _queryBuilder.CreateQuery(requestParameters);
            ConnectToDataFeed();
            

            
            _core.GetData(query, tbFolder.Text,programParameters, (IQFeedRequest)requestParameters, _network);

        }

		private void ReportProgress(KeyValuePair<string, string> type)
		{
			switch (type.Key)
			{
				case "INFO":
					_logger.Info(type.Value);
					break;
				case "WARN":
					_logger.Warn(type.Value);
					break;
				case "ERROR":
					_logger.Error(type.Value);
					break;
				default:
					_logger.Debug(type.Value);
					break;
			}
		}

		#region Controls


        private bool CheckDirectoryAccessControl(string directoryPath){
          
           bool isWriteAccess = false;
           try
           {
              AuthorizationRuleCollection collection = Directory.GetAccessControl(directoryPath).GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
              foreach (FileSystemAccessRule rule in collection)
              {
                 if (rule.AccessControlType == AccessControlType.Allow)
                 {
                    isWriteAccess = true;
                    break;
                 }
              }
           }
           catch (UnauthorizedAccessException ex)
           {
              isWriteAccess = false;

              MessageBox.Show(ex.ToString());

}
           catch (Exception ex)
           {
              isWriteAccess = false;
              MessageBox.Show(ex.ToString());
           }
           return isWriteAccess;
        
}

		private void BtnChooseStoreFolderClick(object sender, EventArgs e)
		{
			var dialog = new FolderBrowserDialog();
			dialog.ShowDialog();
			tbFolder.Text = dialog.SelectedPath;

         
        }

		private void BtnReconnectClick(object sender, EventArgs e)
		{
			ConnectToDataFeed();
		}

		private void RbDaysCheckedChanged(object sender, EventArgs e)
		{
			dtpBeginDate.Enabled = false;
			dtpEndDate.Enabled = false;
			dtpBeginTime.Enabled = false;
			dtpEndTime.Enabled = false;
			tbAmountOfDays.Enabled = true;
		}

		private void RbIntervalCheckedChanged(object sender, EventArgs e)
		{
			dtpBeginDate.Enabled = true;
			dtpEndDate.Enabled = true;
			dtpBeginTime.Enabled = true;
			dtpEndTime.Enabled = true;
			tbAmountOfDays.Enabled = false;
		}

		private void BtnStopClick(object sender, EventArgs e)
		{
			_cts.Cancel();
		}

		#endregion Controls

		#region Private Methods

		private void LockControls()
		{
			btnChooseStoreFolder.Enabled = false;
			rbDays.Enabled = false;
			rbInterval.Enabled = false;
			cbTimeframe.Enabled = false;
			btnReconnect.Enabled = false;
			btnStart.Enabled = false;
			rbMainSession.Enabled = false;
			rbCustomInt.Enabled = false;

			dtpBeginDate.Enabled = false;
			dtpEndDate.Enabled = false;
			dtpBeginTime.Enabled = false;
			dtpEndTime.Enabled = false;
		}

		private void UnlockControls()
		{
			btnChooseStoreFolder.Enabled = true;
			rbDays.Enabled = true;
			rbInterval.Enabled = true;
			cbTimeframe.Enabled = true;
			btnReconnect.Enabled = true;
			btnStart.Enabled = true;
			rbMainSession.Enabled = true;
			rbCustomInt.Enabled = true;

			dtpBeginDate.Enabled = true;
			dtpEndDate.Enabled = true;
			dtpBeginTime.Enabled = true;
			dtpEndTime.Enabled = true;
		}

		private void SetDefaultValues()
		{
			rtbSymbols.Text = Resources.MainForm_DefaultSymbols;
			tbFolder.Text = Resources.MainForm_DefaultPath;

			rbInterval.Enabled = true;
			rbDays.Enabled = true;
			tbAmountOfDays.Text = Resources.MainForm_DefaultAmountOfDays;
			rbDays.Checked = true;

			cbTimeframe.Enabled = true;
			cbTimeframeIntraday.Enabled = false;
			cbTimeframeIntraday.SelectedItem = "1";
			cbTimeframe.SelectedItem = "Intraday";

			dtpBeginDate.Enabled = false;
			dtpEndDate.Enabled = false;
			dtpBeginTime.Enabled = false;
			dtpEndTime.Enabled = false;

			rbMainSession.Checked = false;
			rbCustomInt.Checked = false;
			rbAlldata.Checked = true;

			dtpCustomIntBegin.Enabled = false;
			dtpCustomIntEnd.Enabled = false;

			dtpBeginDate.Value = Convert.ToDateTime(@"01-01-2005");
			dtpEndDate.Value = DateTime.Today;

			dtpBeginTime.Value = Convert.ToDateTime(@"00:00:00");
			dtpEndTime.Value = Convert.ToDateTime(@"23:59:59");

			cbDate.SelectedIndex = 0;
			cbTime.SelectedIndex = 0;
			cbDateTimeSeparator.SelectedIndex = 0;
		}

		private void rbCustomInt_CheckedChanged(object sender, EventArgs e)
		{
			dtpCustomIntBegin.Enabled = true;
			dtpCustomIntEnd.Enabled = true;
		}

		private void rbMainSession_CheckedChanged_1(object sender, EventArgs e)
		{
			dtpCustomIntBegin.Enabled = false;
			dtpCustomIntEnd.Enabled = false;
		}

		private void rbAlldata_CheckedChanged(object sender, EventArgs e)
		{
			dtpCustomIntBegin.Enabled = false;
			dtpCustomIntEnd.Enabled = false;
		}

		private void CbTimeframeSelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cbTimeframe.SelectedItem.ToString())
			{
				case "Tick":
					cbTimeframeIntraday.Enabled = false;
					rbDays.Enabled = true;
					tbAmountOfDays.Enabled = true;
					rbInterval.Enabled = true;

					rbMainSession.Enabled = true;
					rbCustomInt.Enabled = true;
					rbAlldata.Enabled = true;
					break;

				case "Intraday":
					cbTimeframeIntraday.Enabled = true;
					rbDays.Enabled = true;
					tbAmountOfDays.Enabled = true;
					rbInterval.Enabled = true;
					
					rbMainSession.Enabled = true;
					rbCustomInt.Enabled = true;
					rbAlldata.Enabled = true;
					break;

				case "Daily":
					cbTimeframeIntraday.Enabled = false;
                    rbDays.Enabled = true;
                    tbAmountOfDays.Enabled = true;
					rbInterval.Enabled = false;

					rbMainSession.Enabled = false;
					rbCustomInt.Enabled = false;
					rbAlldata.Enabled = false;
					dtpCustomIntBegin.Enabled = false;
					dtpCustomIntEnd.Enabled = false;
					break;

				case "Weekly":
					cbTimeframeIntraday.Enabled = false;
					rbDays.Enabled = false;
					tbAmountOfDays.Enabled = false;
					rbInterval.Enabled = false;
					
					rbMainSession.Enabled = false;
					rbCustomInt.Enabled = false;
					rbAlldata.Enabled = false;
					dtpCustomIntBegin.Enabled = false;
					dtpCustomIntEnd.Enabled = false;
					break;

				case "Monthly":
					cbTimeframeIntraday.Enabled = false;
					rbDays.Enabled = false;
					tbAmountOfDays.Enabled = false;
					rbInterval.Enabled = false;
					
					rbMainSession.Enabled = false;
					rbCustomInt.Enabled = false;
					rbAlldata.Enabled = false;
					dtpCustomIntBegin.Enabled = false;
					dtpCustomIntEnd.Enabled = false;
					break;
			}
		}

		private bool CheckSavePath()
		{
			var isValid = true;
			var path = new PathHelper();

			if (!path.CreateDirectory(tbFolder.Text))
			{
				_logger.Error("Wrong directory name");
				isValid = false;
			}

            if (!CheckDirectoryAccessControl(tbFolder.Text))
            {
                tbFolder.Text = "";
                isValid = false;
            }
            
            return isValid;
		}

		private List<string> GetTickersList()
		{
			return rtbSymbols.Text.Split('\n').ToList();
		}

		#region Connection

		private void ConnectToDataFeed()
		{
			_client = _proxy.Connect();

			if (CheckConnection())
			{
				_network = _proxy.CreateNetworkStream(_client);

               
            }

           
        }

       
        
        public DateTime GetLastDateFromFile(string folder, string symbol,Parameters programParameters)
        {
            string[] SplitString;

            var quotesfilepath = folder + @"\" + symbol + ".txt";
            if (!File.Exists(quotesfilepath)) return new DateTime();
            String last = File.ReadLines(quotesfilepath).Last();
            DateTime LastDataTimeTick = new DateTime();
            SplitString = last.Split(programParameters.OutputDelimiter.ToCharArray());
            try
            {
                //LastDataTimeTick = DateTime.ParseExact(SplitString[0], "dd.MM.yyyy HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                LastDataTimeTick = DateTime.ParseExact(SplitString[0], programParameters.DateFormat + "" + programParameters.TimeFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces);

            }
            catch (Exception e)
            {
                LastDataTimeTick = new DateTime();
                _logger.Error("Can't parse last datetime from file.");
            }

            return LastDataTimeTick;
        }
       
        private bool CheckConnection()
		{
			var isConnected = true;

			if (_client == null || _client.Connected == false)
			{
				isConnected = false;

				statusStrip1.Items[0].Visible = false;
				statusStrip1.Items[1].Visible = true;

				_logger.Error("Can't connect to the data feed. Check IQLink connection");
			}
			else
			{
				statusStrip1.Items[0].Visible = true;
				statusStrip1.Items[1].Visible = false;

				_logger.Warn("Connection established");
			}

			return isConnected;
		}

        
		
        #endregion Connection

        private void cbDate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

		#endregion Private Methods

       
	}
}
