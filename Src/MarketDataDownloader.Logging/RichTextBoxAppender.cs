#region FileHeader

// File:
// MarketDataDownloader/MarketDataDownloader.Logging/RichTextBoxAppender.cs
// 
// Last updated:
// 2013-05-23 6:23 PM

#endregion

#region Usings

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;

#endregion

namespace MarketDataDownloader.Logging
{
	public sealed class RichTextBoxAppender : AppenderSkeleton
	{
		public RichTextBoxAppender(RichTextBox rtb)
		{
			if (rtb == null) throw new ArgumentNullException("rtb");

			rtb.ReadOnly = true;
			rtb.HideSelection = false;
			rtb.Clear();

			RichTextBox = rtb;

			var patterLayout = new PatternLayout
				{
					ConversionPattern = "%d{dd MMM HH:mm:ss} %-5p - %m%n"
				};

			patterLayout.ActivateOptions();

			Layout = patterLayout;
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