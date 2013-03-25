using System;
using System.Globalization;
using System.IO;

namespace IQFeedDownloader
{
	public sealed class PathHelper
	{
		public bool CreateDirectory(string folder)
		{
			if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException("folder");

			bool successfulCreating = Directory.Exists(folder);

			if (IsValidPath(folder))
			{
				Directory.CreateDirectory(folder);
				successfulCreating = true;
			}

			return successfulCreating;
		}

		private bool IsValidPath(string folder)
		{
			char[] invalidPathChars = Path.GetInvalidPathChars();

			bool isValidPath = true;

			foreach (var symbol in invalidPathChars)
			{
				if (folder.Contains(symbol.ToString(CultureInfo.InvariantCulture)))
				{
					isValidPath = false;
				}
			}

			return isValidPath;
		}
	}
}
