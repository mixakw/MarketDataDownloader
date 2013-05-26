// =================================================
// File:
// MarketDataDownloader/MarketDataDownloader.UI/Program.cs
// 
// Last updated:
// 2013-05-24 3:51 PM
// =================================================

#region Usings

using System;
using System.Windows.Forms;

#endregion

namespace MarketDataDownloader.UI
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
