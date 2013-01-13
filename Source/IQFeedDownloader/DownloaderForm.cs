using System;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Collections.Generic;

using IQFeedDownloader.Logger;

using log4net;
using log4net.Config;
using System.ComponentModel;

namespace IQFeedDownloader
{
	public partial class DownloaderForm : Form
	{
		#region Constants

		private const int AMOUNT_OF_DAYS = 7300;

		private const string DEFAULT_PATH = "c:\\IQFeed\\";
		private string DEFAULT_SYMBOL = "@NQ#\n@ES#\n@EU#";

		private const string TICK = "Tick";
		private const string INTRADAY = "Intraday";
		private const string DAILY = "Daily";
		private const string WEEKLY = "Weekly";
		private const string MONTHLY = "Monthly";
		private const string ERROR = "Error";

		private const string TF_DAYS = "Days";
		private const string TF_INTERVAL = "Interval";

		private const char SEPARATOR = '\n';

		#endregion

		#region Variables

		private static ILog _logger;
		private BackgroundWorker _backWorker;

		private int _amountOfDays;
		private string _beginDate;
		private string _endDate;

		private string _timeframeName;
		private string _timeframeType;
		private int _timeframeValue;

		private string _folder;

		private TcpClient _socket;
		private DownloaderHelper _helper;

		#endregion

		#region Constructor

		public DownloaderForm()
		{
			InitializeComponent();

			InitLogger();

			SetDefaultValues();

			if (ConnectToDatafeed())
			{
				_logger.Warn("Connection established");
			}
			else
			{
				_logger.Error("Can't connect to the datafeed. Check IQLink connection");
			}

			InitBackgroudWorker();
		}

		#endregion

		#region Public Interface

		public BackgroundWorker Worker
		{
			get { return _backWorker; }
		} 

		#endregion

		#region BackgroundWorker
		
		private void InitBackgroudWorker()
		{
			_backWorker = new BackgroundWorker();

			_backWorker.DoWork += bWorker_DoWork;
			_backWorker.RunWorkerCompleted += bWorker_RunWorkerCompleted;
			_backWorker.WorkerSupportsCancellation = true;
		}

		private void bWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			var worker = sender as BackgroundWorker;
			if (worker == null) return;

			var parameters = (object[])e.Argument;
			var tickers = (IEnumerable<string>)parameters[0];
			var timeframeName = (string)parameters[1];

			foreach (var ticker in tickers)
			{
				if (worker.CancellationPending)
				{
					e.Cancel = true;
					return;
				}

				ConnectToDatafeed();
				var netStream = _helper.CreateStream(_socket);

				_logger.Info(String.Format("Symbol {0}", ticker));
				var request = _helper.CreateRequest(timeframeName, _timeframeValue, ticker, _amountOfDays, _beginDate, _endDate);
				_helper.GetData(netStream, request, _folder, timeframeName, ticker);
			}

			_logger.Warn("Finished");
		}

		private void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if ((e.Cancelled))
			{
				_logger.Error("Operation was canceled");
			}
			else if (e.Error != null)
			{
				_logger.Error(String.Format("An error occurred: {0}", e.Error.Message));
			}

			UnlockControls();
		}

		#endregion

		#region Private Methods

		private void InitAllVariables()
		{
			if (Int32.TryParse(cbTimeframeIntraday.SelectedItem.ToString(), out _timeframeValue) == false) throw new InvalidCastException("_timeframeValue");

			_beginDate = dtpBeginDate.Value.ToString(CultureInfo.InvariantCulture);
			_endDate = dtpEndDate.Value.ToString(CultureInfo.InvariantCulture);

			if (Int32.TryParse(tbAmountOfDays.Text, out _amountOfDays) == false) throw new InvalidCastException("_amountOfDays");
		}

		private void InitLogger()
		{
			_logger = LogManager.GetLogger(typeof(DownloaderForm));
			XmlConfigurator.Configure();
			RichTextBoxAppender.SetRichTextBox(rtbLog);
		}

		private void SetDefaultValues()
		{
			rtbSymbols.Text = DEFAULT_SYMBOL;

			_folder = DEFAULT_PATH;
			tbFolder.Text = _folder;

			rbInterval.Enabled = true;
			rbDays.Enabled = true;
			_amountOfDays = AMOUNT_OF_DAYS;
			tbAmountOfDays.Text = _amountOfDays.ToString(CultureInfo.InvariantCulture);
			rbDays.Checked = true;

			cbTimeframe.Enabled = true;
			cbTimeframeIntraday.Enabled = false;
			cbTimeframeIntraday.SelectedItem = "1";
			cbTimeframe.SelectedItem = INTRADAY;

			dtpBeginDate.Enabled = false;
			dtpBeginDate.Value = Convert.ToDateTime(@"1/1/2005 12:00:00 AM");
			dtpEndDate.Enabled = false;
			dtpEndDate.Value = DateTime.Today;
		}

		private bool CheckSavePath()
		{
			bool isValid = true;

			_folder = tbFolder.Text;
			var pathHelper = new PathHelper();

			if (!pathHelper.CreateDirectory(_folder))
			{
				_logger.Error("Wrong directory name");
				isValid = false;
			}

			return isValid;
		}

		private bool ConnectToDatafeed()
		{
			bool isSuccessful = false;
			var helper = new DownloaderHelper();

			var socket = helper.OpenTcpClientConnection();

			if (socket == null)
			{
				statusStrip1.Items[0].Visible = false;
				statusStrip1.Items[1].Visible = true;
			}

			if (socket != null)
			{
				statusStrip1.Items[0].Visible = true;
				statusStrip1.Items[1].Visible = false;
				isSuccessful = true;
			}

			_helper = helper;
			_socket = socket;

			return isSuccessful;
		}

		private IEnumerable<string> GetSymbols()
		{
			return rtbSymbols.Text.Split(SEPARATOR);
		}

		private string GetTimeframeName()
		{
			return _timeframeName + " " + _timeframeType;
		}

		private void LockControls()
		{
			btnChooseStoreFolder.Enabled = false;
			rbDays.Enabled = false;
			rbInterval.Enabled = false;
			cbTimeframe.Enabled = false;
			btnReconnect.Enabled = false;
			btnStart.Enabled = false;
		}

		private void UnlockControls()
		{
			btnChooseStoreFolder.Enabled = true;
			rbDays.Enabled = true;
			rbInterval.Enabled = true;
			cbTimeframe.Enabled = true;
			btnReconnect.Enabled = true;
			btnStart.Enabled = true;
		}

		#endregion

		#region Form Controls Events

		private void cbTimeframe_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cbTimeframe.SelectedItem.ToString())
			{
				case TICK:
					_timeframeName = TICK;
					cbTimeframeIntraday.Enabled = false;
					rbDays.Enabled = true;
					tbAmountOfDays.Enabled = true;
					rbInterval.Enabled = true;
					break;

				case INTRADAY:
					_timeframeName = INTRADAY;
					cbTimeframeIntraday.Enabled = true;
					rbDays.Enabled = true;
					tbAmountOfDays.Enabled = true;
					rbInterval.Enabled = true;
					break;

				case DAILY:
					_timeframeName = DAILY;
					cbTimeframeIntraday.Enabled = false;
					rbDays.Enabled = false;
					tbAmountOfDays.Enabled = false;
					rbInterval.Enabled = false;
					break;

				case WEEKLY:
					_timeframeName = WEEKLY;
					cbTimeframeIntraday.Enabled = false;
					rbDays.Enabled = false;
					tbAmountOfDays.Enabled = false;
					rbInterval.Enabled = false;
					break;

				case MONTHLY:
					_timeframeName = MONTHLY;
					cbTimeframeIntraday.Enabled = false;
					rbDays.Enabled = false;
					tbAmountOfDays.Enabled = false;
					rbInterval.Enabled = false;
					break;

				default:
					_timeframeName = ERROR;
					cbTimeframeIntraday.Enabled = true;
					rbDays.Enabled = true;
					tbAmountOfDays.Enabled = true;
					rbInterval.Enabled = true;
					break;
			}
		}

		private void rbDays_CheckedChanged(object sender, EventArgs e)
		{
			dtpBeginDate.Enabled = false;
			dtpEndDate.Enabled = false;
			tbAmountOfDays.Enabled = true;
			_timeframeType = TF_DAYS;
		}

		private void rbInterval_CheckedChanged(object sender, EventArgs e)
		{
			dtpBeginDate.Enabled = true;
			dtpEndDate.Enabled = true;
			tbAmountOfDays.Enabled = false;
			_timeframeType = TF_INTERVAL;
		}

		#endregion

		#region Buttons Events

		private void btnChooseStoreFolder_Click(object sender, EventArgs e)
		{
			var folderbrowserdialog = new FolderBrowserDialog();
			folderbrowserdialog.ShowDialog();
			_folder = folderbrowserdialog.SelectedPath;
			tbFolder.Text = _folder;
		}

		private void btnReconnect_Click(object sender, EventArgs e)
		{
			if (ConnectToDatafeed())
			{
				_logger.Warn("Connection established");
			}
			else
			{
				_logger.Error("Can't connect to the datafeed. Check IQLink connection");
			}
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			_logger.Warn("Process will stop at the next symbol. Please wait");

			if (_backWorker.WorkerSupportsCancellation)
			{
				_backWorker.CancelAsync();
			}
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			if (_backWorker.IsBusy != true)
			{
				if (!CheckSavePath()) return;

				InitAllVariables();

				if (_socket == null || _socket.Connected == false)
				{
					_logger.Info("Check datafeed connection");
					return;
				}

				LockControls();

				_logger.Info("Starting processing");

				var parameters = new object[2];
				parameters[0] = GetSymbols().ToList();
				parameters[1] = GetTimeframeName();

				_backWorker.RunWorkerAsync(parameters);
			}
			else
			{
				_logger.Info("Previous process is running. Restart application");
			}
		}

		#endregion
	}
}
