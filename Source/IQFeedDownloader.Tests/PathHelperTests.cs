using System;
using Xunit;

namespace IQFeedDownloader.Tests
{
	public class PathHelperTests
	{
		[Fact]
		public void BadMethod()
		{
			PathHelper ph = new PathHelper();
			bool result = ph.CreateDirectory(@"C:\Folder\");

			Assert.Equal(true, result);
		}
	}
}
