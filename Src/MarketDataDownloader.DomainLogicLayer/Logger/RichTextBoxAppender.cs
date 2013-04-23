using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;

using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using log4net.Core;

namespace MarketDataDownloader.DomainLogicLayer.Logger
{
	public class RichTextBoxAppender : AppenderSkeleton
	{
		public RichTextBox RichTextBox { get; set; }

		public RichTextBoxAppender(RichTextBox rtb)
		{
			if (rtb == null) throw new ArgumentNullException("rtb");

			rtb.ReadOnly = true;
			rtb.HideSelection = false;
			rtb.Clear();

			foreach (var appender in GetAppenders())
			{
				var richTextBoxAppender = appender as RichTextBoxAppender;

				if (richTextBoxAppender != null)
				{
					richTextBoxAppender.RichTextBox = rtb;
				}
			}
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			if (loggingEvent == null) throw new ArgumentNullException("loggingEvent");

			if (RichTextBox.InvokeRequired)
			{
				RichTextBox.Invoke(new UpdateControlDelegate(UpdateControl), loggingEvent);
			}
			else
			{
				UpdateControl(loggingEvent);
			}
		}

		private IEnumerable<IAppender> GetAppenders()
		{
			var appenders = new ArrayList();

			appenders.AddRange(((Hierarchy)LogManager.GetRepository()).Root.Appenders);

			foreach (var log in LogManager.GetCurrentLoggers())
			{
				appenders.AddRange(((log4net.Repository.Hierarchy.Logger)log.Logger).Appenders);
			}

			return (IAppender[])appenders.ToArray(typeof(IAppender));
		}

		private delegate void UpdateControlDelegate(LoggingEvent loggingEvent);

		private void UpdateControl(LoggingEvent loggingEvent)
		{
			if (RichTextBox.TextLength > 100000)
			{
				RichTextBox.Clear();
				RichTextBox.SelectionColor = Color.Gray;
				RichTextBox.AppendText("(earlier messages cleared because of log length)\n\n");
			}

			switch (loggingEvent.Level.ToString())
			{
				case "INFO":
					RichTextBox.SelectionColor = Color.Black;
					break;
				case "WARN":
					RichTextBox.SelectionColor = Color.Green;
					break;
				case "ERROR":
					RichTextBox.SelectionColor = Color.DarkRed;
					break;
				default:
					RichTextBox.SelectionColor = Color.DarkRed;
					break;
			}

			using (var sw = new StringWriter())
			{
				Layout.Format(sw, loggingEvent);
				RichTextBox.AppendText(sw.ToString());
			}
		}
	}
}
