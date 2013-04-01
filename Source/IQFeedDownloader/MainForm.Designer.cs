namespace IQFeedDownloader
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.label2 = new System.Windows.Forms.Label();
			this.btnChooseStoreFolder = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chbMainSession = new System.Windows.Forms.CheckBox();
			this.rbInterval = new System.Windows.Forms.RadioButton();
			this.cbTimeframe = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.rbDays = new System.Windows.Forms.RadioButton();
			this.label11 = new System.Windows.Forms.Label();
			this.tbFolder = new System.Windows.Forms.TextBox();
			this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
			this.cbTimeframeIntraday = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.tbAmountOfDays = new System.Windows.Forms.TextBox();
			this.dtpBeginDate = new System.Windows.Forms.DateTimePicker();
			this.btnStart = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rtbSymbols = new System.Windows.Forms.RichTextBox();
			this.rtbLog = new System.Windows.Forms.RichTextBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnReconnect = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tbFeeds = new System.Windows.Forms.TabControl();
			this.tabIQFeed = new System.Windows.Forms.TabPage();
			this.tabSetup = new System.Windows.Forms.TabPage();
			this.cbDateTimeSeparator = new System.Windows.Forms.ComboBox();
			this.cbTime = new System.Windows.Forms.ComboBox();
			this.cbDate = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.statusStrip1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tbFeeds.SuspendLayout();
			this.tabIQFeed.SuspendLayout();
			this.tabSetup.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 19);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(73, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Save to folder";
			// 
			// btnChooseStoreFolder
			// 
			this.btnChooseStoreFolder.Location = new System.Drawing.Point(94, 14);
			this.btnChooseStoreFolder.Name = "btnChooseStoreFolder";
			this.btnChooseStoreFolder.Size = new System.Drawing.Size(197, 23);
			this.btnChooseStoreFolder.TabIndex = 2;
			this.btnChooseStoreFolder.Text = "Choose...";
			this.btnChooseStoreFolder.UseVisualStyleBackColor = true;
			this.btnChooseStoreFolder.Click += new System.EventHandler(this.BtnChooseStoreFolderClick);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "History Depth";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
			this.statusStrip1.Location = new System.Drawing.Point(0, 553);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(502, 22);
			this.statusStrip1.TabIndex = 10;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Green;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(117, 17);
			this.toolStripStatusLabel1.Text = "IQFeed CONNECTED";
			this.toolStripStatusLabel1.Visible = false;
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.Red;
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(134, 17);
			this.toolStripStatusLabel2.Text = "IQFeed DISCONNECTED";
			this.toolStripStatusLabel2.Visible = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.chbMainSession);
			this.groupBox2.Controls.Add(this.rbInterval);
			this.groupBox2.Controls.Add(this.cbTimeframe);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.rbDays);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.tbFolder);
			this.groupBox2.Controls.Add(this.dtpEndDate);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.cbTimeframeIntraday);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.tbAmountOfDays);
			this.groupBox2.Controls.Add(this.dtpBeginDate);
			this.groupBox2.Controls.Add(this.btnChooseStoreFolder);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(159, 6);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(300, 228);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Settings";
			// 
			// chbMainSession
			// 
			this.chbMainSession.AutoSize = true;
			this.chbMainSession.Location = new System.Drawing.Point(93, 195);
			this.chbMainSession.Name = "chbMainSession";
			this.chbMainSession.Size = new System.Drawing.Size(204, 17);
			this.chbMainSession.TabIndex = 35;
			this.chbMainSession.Text = "Only main session (9:30 am - 4:00 pm)";
			this.chbMainSession.UseVisualStyleBackColor = true;
			// 
			// rbInterval
			// 
			this.rbInterval.AutoSize = true;
			this.rbInterval.Location = new System.Drawing.Point(94, 93);
			this.rbInterval.Name = "rbInterval";
			this.rbInterval.Size = new System.Drawing.Size(60, 17);
			this.rbInterval.TabIndex = 34;
			this.rbInterval.TabStop = true;
			this.rbInterval.Text = "Interval";
			this.rbInterval.UseVisualStyleBackColor = true;
			this.rbInterval.CheckedChanged += new System.EventHandler(this.RbIntervalCheckedChanged);
			// 
			// cbTimeframe
			// 
			this.cbTimeframe.FormattingEnabled = true;
			this.cbTimeframe.Items.AddRange(new object[] {
            "Tick",
            "Intraday",
            "Daily",
            "Weekly",
            "Monthly"});
			this.cbTimeframe.Location = new System.Drawing.Point(94, 168);
			this.cbTimeframe.Name = "cbTimeframe";
			this.cbTimeframe.Size = new System.Drawing.Size(116, 21);
			this.cbTimeframe.TabIndex = 29;
			this.cbTimeframe.SelectedIndexChanged += new System.EventHandler(this.CbTimeframeSelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(8, 171);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(56, 13);
			this.label6.TabIndex = 21;
			this.label6.Text = "Timeframe";
			// 
			// rbDays
			// 
			this.rbDays.AutoSize = true;
			this.rbDays.Location = new System.Drawing.Point(94, 70);
			this.rbDays.Name = "rbDays";
			this.rbDays.Size = new System.Drawing.Size(100, 17);
			this.rbDays.TabIndex = 33;
			this.rbDays.TabStop = true;
			this.rbDays.Text = "Amount of Days";
			this.rbDays.UseVisualStyleBackColor = true;
			this.rbDays.CheckedChanged += new System.EventHandler(this.RbDaysCheckedChanged);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(6, 122);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(60, 13);
			this.label11.TabIndex = 28;
			this.label11.Text = "Begin Date";
			// 
			// tbFolder
			// 
			this.tbFolder.Location = new System.Drawing.Point(94, 44);
			this.tbFolder.Name = "tbFolder";
			this.tbFolder.Size = new System.Drawing.Size(197, 20);
			this.tbFolder.TabIndex = 20;
			// 
			// dtpEndDate
			// 
			this.dtpEndDate.Location = new System.Drawing.Point(94, 141);
			this.dtpEndDate.Name = "dtpEndDate";
			this.dtpEndDate.Size = new System.Drawing.Size(197, 20);
			this.dtpEndDate.TabIndex = 31;
			// 
			// cbTimeframeIntraday
			// 
			this.cbTimeframeIntraday.FormattingEnabled = true;
			this.cbTimeframeIntraday.Items.AddRange(new object[] {
            "1",
            "5",
            "10",
            "15",
            "30",
            "60",
            "120",
            "180",
            "240"});
			this.cbTimeframeIntraday.Location = new System.Drawing.Point(216, 167);
			this.cbTimeframeIntraday.Name = "cbTimeframeIntraday";
			this.cbTimeframeIntraday.Size = new System.Drawing.Size(75, 21);
			this.cbTimeframeIntraday.TabIndex = 15;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(6, 147);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(52, 13);
			this.label12.TabIndex = 29;
			this.label12.Text = "End Date";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 46);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(39, 13);
			this.label8.TabIndex = 14;
			this.label8.Text = "Folder:";
			// 
			// tbAmountOfDays
			// 
			this.tbAmountOfDays.Location = new System.Drawing.Point(200, 67);
			this.tbAmountOfDays.Name = "tbAmountOfDays";
			this.tbAmountOfDays.Size = new System.Drawing.Size(91, 20);
			this.tbAmountOfDays.TabIndex = 31;
			// 
			// dtpBeginDate
			// 
			this.dtpBeginDate.Location = new System.Drawing.Point(94, 116);
			this.dtpBeginDate.Name = "dtpBeginDate";
			this.dtpBeginDate.Size = new System.Drawing.Size(197, 20);
			this.dtpBeginDate.TabIndex = 30;
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(263, 248);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(90, 25);
			this.btnStart.TabIndex = 14;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.BtnStartClick);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.rtbSymbols);
			this.groupBox3.Location = new System.Drawing.Point(6, 6);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(147, 231);
			this.groupBox3.TabIndex = 17;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Symbols";
			// 
			// rtbSymbols
			// 
			this.rtbSymbols.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbSymbols.Location = new System.Drawing.Point(3, 16);
			this.rtbSymbols.Name = "rtbSymbols";
			this.rtbSymbols.Size = new System.Drawing.Size(141, 212);
			this.rtbSymbols.TabIndex = 0;
			this.rtbSymbols.Text = "";
			// 
			// rtbLog
			// 
			this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtbLog.Location = new System.Drawing.Point(12, 336);
			this.rtbLog.Name = "rtbLog";
			this.rtbLog.ReadOnly = true;
			this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.rtbLog.Size = new System.Drawing.Size(478, 214);
			this.rtbLog.TabIndex = 1;
			this.rtbLog.Text = "";
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(369, 248);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(90, 25);
			this.btnStop.TabIndex = 19;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.BtnStopClick);
			// 
			// btnReconnect
			// 
			this.btnReconnect.Location = new System.Drawing.Point(156, 248);
			this.btnReconnect.Name = "btnReconnect";
			this.btnReconnect.Size = new System.Drawing.Size(90, 25);
			this.btnReconnect.TabIndex = 20;
			this.btnReconnect.Text = "Reconnect";
			this.btnReconnect.UseVisualStyleBackColor = true;
			this.btnReconnect.Click += new System.EventHandler(this.BtnReconnectClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 320);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(75, 13);
			this.label1.TabIndex = 21;
			this.label1.Text = "Log messages";
			// 
			// tbFeeds
			// 
			this.tbFeeds.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbFeeds.Controls.Add(this.tabIQFeed);
			this.tbFeeds.Controls.Add(this.tabSetup);
			this.tbFeeds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tbFeeds.Location = new System.Drawing.Point(12, 12);
			this.tbFeeds.Name = "tbFeeds";
			this.tbFeeds.SelectedIndex = 0;
			this.tbFeeds.Size = new System.Drawing.Size(478, 305);
			this.tbFeeds.TabIndex = 22;
			// 
			// tabIQFeed
			// 
			this.tabIQFeed.Controls.Add(this.groupBox3);
			this.tabIQFeed.Controls.Add(this.groupBox2);
			this.tabIQFeed.Controls.Add(this.btnStart);
			this.tabIQFeed.Controls.Add(this.btnStop);
			this.tabIQFeed.Controls.Add(this.btnReconnect);
			this.tabIQFeed.Location = new System.Drawing.Point(4, 22);
			this.tabIQFeed.Name = "tabIQFeed";
			this.tabIQFeed.Padding = new System.Windows.Forms.Padding(3);
			this.tabIQFeed.Size = new System.Drawing.Size(470, 279);
			this.tabIQFeed.TabIndex = 0;
			this.tabIQFeed.Text = "IQFeed";
			this.tabIQFeed.UseVisualStyleBackColor = true;
			// 
			// tabSetup
			// 
			this.tabSetup.Controls.Add(this.cbDateTimeSeparator);
			this.tabSetup.Controls.Add(this.cbTime);
			this.tabSetup.Controls.Add(this.cbDate);
			this.tabSetup.Controls.Add(this.label3);
			this.tabSetup.Location = new System.Drawing.Point(4, 22);
			this.tabSetup.Name = "tabSetup";
			this.tabSetup.Padding = new System.Windows.Forms.Padding(3);
			this.tabSetup.Size = new System.Drawing.Size(470, 279);
			this.tabSetup.TabIndex = 1;
			this.tabSetup.Text = "Parameters";
			this.tabSetup.UseVisualStyleBackColor = true;
			// 
			// cbDateTimeSeparator
			// 
			this.cbDateTimeSeparator.FormattingEnabled = true;
			this.cbDateTimeSeparator.Items.AddRange(new object[] {
            " ",
            "\t",
            ",",
            ".",
            ":",
            ";",
            "/",
            "\\",
            "-"});
			this.cbDateTimeSeparator.Location = new System.Drawing.Point(221, 9);
			this.cbDateTimeSeparator.Name = "cbDateTimeSeparator";
			this.cbDateTimeSeparator.Size = new System.Drawing.Size(61, 21);
			this.cbDateTimeSeparator.TabIndex = 3;
			// 
			// cbTime
			// 
			this.cbTime.FormattingEnabled = true;
			this.cbTime.Items.AddRange(new object[] {
            "H:mm:ss",
            "H.mm",
            "H.mm.ss",
            "H:mm",
            "Hmm",
            "Hmmss",
            "hh.mm tt",
            "hh.mm.ss tt",
            "hh:mm tt",
            "hh:mm:ss tt",
            "hhmm tt",
            "hhmmss tt"});
			this.cbTime.Location = new System.Drawing.Point(288, 9);
			this.cbTime.Name = "cbTime";
			this.cbTime.Size = new System.Drawing.Size(120, 21);
			this.cbTime.TabIndex = 2;
			// 
			// cbDate
			// 
			this.cbDate.FormattingEnabled = true;
			this.cbDate.Items.AddRange(new object[] {
            "yyyy-MM-dd",
            "d/M/yyyy",
            "ddMMyyyy",
            "d-MMM-yyyy",
            "d-M-yyyy",
            "d.M.yy",
            "dd.MM.yyyy",
            "M/d/yyyy",
            "M-d-yyyy",
            "MMddyy",
            "MMddyyyy",
            "yyddMM",
            "yyMMdd",
            "yyyy/MM/dd",
            "yyyyddMM",
            "yyyyMMdd"});
			this.cbDate.Location = new System.Drawing.Point(95, 9);
			this.cbDate.Name = "cbDate";
			this.cbDate.Size = new System.Drawing.Size(120, 21);
			this.cbDate.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "DateTime Format";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(502, 575);
			this.Controls.Add(this.tbFeeds);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.rtbLog);
			this.Controls.Add(this.statusStrip1);
			this.Name = "MainForm";
			this.Text = "Market Data Downloader 2.1 (by AnCh)";
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.tbFeeds.ResumeLayout(false);
			this.tabIQFeed.ResumeLayout(false);
			this.tabSetup.ResumeLayout(false);
			this.tabSetup.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChooseStoreFolder;
		private System.Windows.Forms.Label label4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cbTimeframeIntraday;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.RichTextBox rtbSymbols;
        private System.Windows.Forms.TextBox tbFolder;
        private System.Windows.Forms.Button btnReconnect;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbTimeframe;
        private System.Windows.Forms.RadioButton rbInterval;
        private System.Windows.Forms.RadioButton rbDays;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbAmountOfDays;
		private System.Windows.Forms.DateTimePicker dtpBeginDate;
		private System.Windows.Forms.TabControl tbFeeds;
		private System.Windows.Forms.TabPage tabIQFeed;
		private System.Windows.Forms.TabPage tabSetup;
		private System.Windows.Forms.ComboBox cbTime;
		private System.Windows.Forms.ComboBox cbDate;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbDateTimeSeparator;
		private System.Windows.Forms.CheckBox chbMainSession;
    }
}

