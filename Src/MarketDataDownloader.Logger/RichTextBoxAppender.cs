#region FileHeader

// File:
// MarketDataDownloader/MarketDataDownloader.Logger/RichTextBoxAppender.cs
// 
// Last updated:
// 2013-05-23 10:44 AM

#endregion

#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;

#endregion

namespace MarketDataDownloader.Logger
{
	public class RichTextBoxAppender : AppenderSkeleton
	{
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

		public RichTextBox RichTextBox { get; set; }

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

			appenders.AddRange(((Hierarchy) LogManager.GetRepository()).Root.Appenders);

			foreach (var log in LogManager.GetCurrentLoggers())
			{
				appenders.AddRange(((log4net.Repository.Hierarchy.Logger) log.Logger).Appenders);
			}

			return (IAppender[]) appenders.ToArray(typeof (IAppender));
		}

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

		private delegate void UpdateControlDelegate(LoggingEvent loggingEvent);
	}
}