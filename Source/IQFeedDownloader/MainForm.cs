using System;
using System.Globalization;
using System.Windows.Forms;

using IQFeedDownloader.Logger;
using IQFeedDownloader.Properties;

using log4net;
using log4net.Config;
using System.ComponentModel;

namespace IQFeedDownloader
{
	public partial class MainForm : Form
	{
		#region Variables

		private static ILog _logger;
		private BackgroundWorker _backWorker;

		private Helper _helper;

		private Request _request;
		private Response _response;
		private Parameters _parameters;

		#endregion Variables

		#region Constructor

		public MainForm()
		{
			InitializeComponent();

			InitLogger();

			ConnectToDataFeed();

			SetDefaultValues();

			LoadParameters();

			InitBackgroudWorker();
		}

		#endregion Constructor

		#region BackgroundWorker

		public BackgroundWorker Worker
		{
			get { return _backWorker; }
		}

		private void InitBackgroudWorker()
		{
			_backWorker = new BackgroundWorker();

			_backWorker.DoWork += BWorkerDoWork;
			_backWorker.RunWorkerCompleted += BWorkerRunWorkerCompleted;
			_backWorker.WorkerSupportsCancellation = true;
		}

		private void BWorkerDoWork(object sender, DoWorkEventArgs e)
		{
			var worker = sender as BackgroundWorker;
			if (worker == null)
				return;

			foreach (var symbol in _request.Symbols)
			{
				if (worker.CancellationPending)
				{
					e.Cancel = true;
					return;
				}

				_logger.Info(String.Format("Symbol {0}", symbol));
				string query = _helper.CreateQuery(_request);
				_helper.GetData(query, tbFolder.Text, _request);
			}

			_logger.Warn("Finished");
		}

		private void BWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

		private void InitLogger()
		{
			_logger = LogManager.GetLogger(typeof(MainForm));
			XmlConfigurator.Configure();
			RichTextBoxAppender.SetRichTextBox(rtbLog);
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

			dtpBeginDate.Value = Convert.ToDateTime(@"1/1/2005 12:00:00 AM");
			dtpEndDate.Value = DateTime.Today;

			cbDate.SelectedIndex = 0;
			cbTime.SelectedIndex = 0;
			cbDateTimeSeparator.SelectedIndex = 0;
		}

		private bool CheckSavePath()
		{
			bool isValid = true;

			PathHelper path = new PathHelper();

			if (!path.CreateDirectory(tbFolder.Text))
			{
				_logger.Error("Wrong directory name");
				isValid = false;
			}

			return isValid;
		}

		private void LockControls()
		{
			btnChooseStoreFolder.Enabled = false;
			rbDays.Enabled = false;
			rbInterval.Enabled = false;
			cbTimeframe.Enabled = false;
			btnReconnect.Enabled = false;
			btnStart.Enabled = false;
			chbMainSession.Enabled = false;
		}

		private void UnlockControls()
		{
			btnChooseStoreFolder.Enabled = true;
			rbDays.Enabled = true;
			rbInterval.Enabled = true;
			cbTimeframe.Enabled = true;
			btnReconnect.Enabled = true;
			btnStart.Enabled = true;
			chbMainSession.Enabled = true;
		}

		private void LoadParameters()
		{
			_parameters = new Parameters { DateFormat = cbDate.Text, TimeFormat = cbTime.Text, DateTimeDelimeter = cbDateTimeSeparator.Text };

			_request = new Request
						   {
							   TimeFrameName = cbTimeframe.SelectedItem.ToString(),
							   Interval = Int32.Parse(cbTimeframeIntraday.SelectedItem.ToString()),
							   Days = tbAmountOfDays.Text
						   };

			if (rbDays.Checked)
				_request.TimeFrameType = "Days";

			if (rbInterval.Checked)
				_request.TimeFrameType = "Interval";

			_request.BeginDate = dtpBeginDate.Value.ToString(CultureInfo.InvariantCulture);
			_request.EndDate = dtpEndDate.Value.ToString(CultureInfo.InvariantCulture);

			_request.Symbols.AddRange(rtbSymbols.Text.Split('\n'));
		}

		private void ConnectToDataFeed()
		{
			if (_helper == null)
				_helper = new Helper();

			_helper.OpenConnection();

			if (_helper.Socket == null)
			{
				statusStrip1.Items[0].Visible = false;
				statusStrip1.Items[1].Visible = true;
				_logger.Error("Can't connect to the datafeed. Check IQLink connection");
			}
			else
			{
				statusStrip1.Items[0].Visible = true;
				statusStrip1.Items[1].Visible = false;
				_logger.Warn("Connection established");
			}
		}

		#endregion

		#region Form Controls Events

		private void CbTimeframeSelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cbTimeframe.SelectedItem.ToString())
			{
				case "Tick":
					cbTimeframeIntraday.Enabled = false;
					rbDays.Enabled = true;
					tbAmountOfDays.Enabled = true;
					rbInterval.Enabled = true;
					chbMainSession.Checked = false;
					chbMainSession.Enabled = true;
					break;

				case "Intraday":
					cbTimeframeIntraday.Enabled = true;
					rbDays.Enabled = true;
					tbAmountOfDays.Enabled = true;
					rbInterval.Enabled = true;
					chbMainSession.Checked = false;
					chbMainSession.Enabled = true;
					break;

				case "Daily":
					cbTimeframeIntraday.Enabled = false;
					rbDays.Enabled = false;
					tbAmountOfDays.Enabled = false;
					rbInterval.Enabled = false;
					chbMainSession.Checked = false;
					chbMainSession.Enabled = false;
					break;

				case "Weekly":
					cbTimeframeIntraday.Enabled = false;
					rbDays.Enabled = false;
					tbAmountOfDays.Enabled = false;
					rbInterval.Enabled = false;
					chbMainSession.Checked = false;
					chbMainSession.Enabled = false;
					break;

				case "Monthly":
					cbTimeframeIntraday.Enabled = false;
					rbDays.Enabled = false;
					tbAmountOfDays.Enabled = false;
					rbInterval.Enabled = false;
					chbMainSession.Checked = false;
					chbMainSession.Enabled = false;
					break;
			}
		}

		private void RbDaysCheckedChanged(object sender, EventArgs e)
		{
			dtpBeginDate.Enabled = false;
			dtpEndDate.Enabled = false;
			tbAmountOfDays.Enabled = true;
		}

		private void RbIntervalCheckedChanged(object sender, EventArgs e)
		{
			dtpBeginDate.Enabled = true;
			dtpEndDate.Enabled = true;
			tbAmountOfDays.Enabled = false;
		}

		#endregion Form Controls Events

		#region Buttons Events

		private void BtnChooseStoreFolderClick(object sender, EventArgs e)
		{
			var dialog = new FolderBrowserDialog();
			dialog.ShowDialog();
			tbFolder.Text = dialog.SelectedPath;
		}

		private void BtnStopClick(object sender, EventArgs e)
		{
			_logger.Warn("Process will stop at the next symbol...");

			if (_backWorker.WorkerSupportsCancellation)
			{
				_backWorker.CancelAsync();
			}
		}

		private void BtnStartClick(object sender, EventArgs e)
		{
			LockControls();
			LoadParameters();

			bool correctPath = CheckSavePath();

			if (correctPath)
			{
				if (_backWorker.IsBusy)
				{
					_logger.Info("Previous process is running");
				}
				else
				{
					if (_helper.Socket == null || _helper.Socket.Connected == false)
					{
						_logger.Info("Check datafeed connection");
					}
					else
					{
						_logger.Info("Starting processing");
						_backWorker.RunWorkerAsync();
					}
				}
			}
		}

		private void BtnReconnectClick(object sender, EventArgs e)
		{
			ConnectToDataFeed();
		}

		#endregion Buttons Events
	}
}