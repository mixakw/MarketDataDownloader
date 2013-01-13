using System;
using System.Globalization;
using System.IO;

namespace IQFeedDownloader
{
	public sealed class PathHelper
	{
		public bool CreateDirectory(string folder)
		{
			if (folder == null) throw new ArgumentNullException("folder");

			bool successfulCreating = Directory.Exists(folder);

			if (IsValidPath(folder))
			{
				Directory.CreateDirectory(folder);
				successfulCreating = true;
			}

			return successfulCreating;
		}

		private bool IsValidPath(string foldername)
		{
			var invalidPathChars = Path.GetInvalidPathChars();

			bool isValidPath = true;

			foreach (var symbol in invalidPathChars)
			{
				if (foldername.Contains(symbol.ToString(CultureInfo.InvariantCulture)))
				{
					isValidPath = false;
				}
			}

			return isValidPath;
		}
	}
}
