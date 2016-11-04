using System;
using System.Collections.Generic;

using Xunit;

namespace Tiandao.IO.Test
{
    public class PathTest
    {
		[Fact]
	    public void PathParseTest()
	    {
			var text = @"zfs.local:/data/images/1/year/month-day/[1]123.jpg";
			var path = Path.Parse(text);
			Assert.Equal("zfs.local", path.Scheme);
			Assert.Equal("/data/images/1/year/month-day/[1]123.jpg", path.FullPath);
			Assert.Equal("/data/images/1/year/month-day/", path.DirectoryName);
			Assert.Equal("[1]123.jpg", path.FileName);

			Assert.True(Path.TryParse("/images/avatar/large/steve.jpg", out path));

			Assert.Null(path.Scheme);
			Assert.True(path.IsFile);
			Assert.True(Path.TryParse("zs:", out path));
			Assert.Equal("zs", path.Scheme);
			Assert.True(path.IsDirectory);
			Assert.Equal("/", path.FullPath);
			Assert.Equal("zs:/", path.Url);
	    }
    }
}
