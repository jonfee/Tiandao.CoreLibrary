using System;
using System.Collections.Generic;

using Xunit;

namespace Tiandao.LBS.Test
{
    public class LocationUtilityTest
    {
		[Fact]
		public void EqualsTest()
	    {
			var location1 = new Location(22.518023, 113.928922);
			var location2 = new Location(22.518023, 113.928922);

			Assert.Equal(location1, location2);
	    }

	    [Fact]
		public void ParseTest()
		{
			Assert.NotNull(LocationUtility.Parse("22.4977230000,113.9220230000"));
			Assert.Null(LocationUtility.Parse("22.4977230000[113.9220230000]"));
		}

		[Fact]
		public void GetDistanceTest()
		{
			// 采用百度坐标(09)
			Assert.Equal(0.5575, LocationUtility.GetDistance(22.4977230000, 113.9220230000, 22.4943320000, 113.9260120000));

			// 采用高德腾讯坐标(02)
			Assert.Equal(0.5432, LocationUtility.GetDistance(22.4918523806, 113.9155480930, 22.4885799188, 113.9194658604));

			// 采用硬件坐标(84)
			Assert.Equal(0.5437, LocationUtility.GetDistance(new Location(22.4948556106, 113.9106398730), new Location(22.4916106488, 113.9145906104)));
		}

		[Fact]
	    public void GetPositionTest()
		{
			var a = LocationUtility.GetPosition(22.4977230000, 113.9220230000, 0.5);
			var b = LocationUtility.GetPosition(new Location(22.4977230000, 113.9220230000), 0.5);
		}

		[Fact]
	    public void IsInRangeTest()
		{
			Assert.True(LocationUtility.IsInRange(new Location(22.4943320000, 113.9260120000), new Location(22.4977230000, 113.9220230000), 1));
			Assert.True(new Location(22.4943320000, 113.9260120000).IsInRange(22.4977230000, 113.9220230000, 1));
		}

		[Fact]
	    public void IsInPolygonTest()
		{
			var point = new Location(22.409439, 113.735414);

			// 深圳市南山区
			var points = new List<Location>();
			points.Add(new Location(22.506303, 113.997266));
			points.Add(new Location(22.513987, 114.006482));
			points.Add(new Location(22.529682, 113.999479));
			points.Add(new Location(22.539055, 114.003994));
			points.Add(new Location(22.55302, 114.00266));
			points.Add(new Location(22.555931, 114.006308));
			points.Add(new Location(22.560939, 114.003886));
			points.Add(new Location(22.562909, 114.012577));
			points.Add(new Location(22.570114, 114.00892));
			points.Add(new Location(22.582217, 114.012039));
			points.Add(new Location(22.589529, 114.030786));
			points.Add(new Location(22.606064, 114.029787));
			points.Add(new Location(22.612017, 114.019419));
			points.Add(new Location(22.63413, 114.009938));
			points.Add(new Location(22.635255, 114.001227));
			points.Add(new Location(22.643759, 113.997029));
			points.Add(new Location(22.644023, 113.981667));
			points.Add(new Location(22.648414, 113.974815));
			points.Add(new Location(22.656977, 113.973232));
			points.Add(new Location(22.658642, 113.956087));
			points.Add(new Location(22.655666, 113.949057));
			points.Add(new Location(22.643623, 113.943025));
			points.Add(new Location(22.640058, 113.936492));
			points.Add(new Location(22.613872, 113.937102));
			points.Add(new Location(22.604535, 113.931339));
			points.Add(new Location(22.598716, 113.933979));
			points.Add(new Location(22.593551, 113.930993));
			points.Add(new Location(22.58658, 113.936156));
			points.Add(new Location(22.579901, 113.933197));
			points.Add(new Location(22.571698, 113.922604));
			points.Add(new Location(22.562977, 113.922503));
			points.Add(new Location(22.547586, 113.900917));
			points.Add(new Location(22.531673, 113.857115));
			points.Add(new Location(22.516216, 113.836299));
			points.Add(new Location(22.409449, 113.735253));
			points.Add(new Location(22.398465, 113.764399));
			points.Add(new Location(22.371004, 113.79039));
			points.Add(new Location(22.347731, 113.802364));
			points.Add(new Location(22.290004, 113.813838));
			points.Add(new Location(22.240159, 113.837116));
			points.Add(new Location(22.270685, 113.850441));
			points.Add(new Location(22.276198, 113.858777));
			points.Add(new Location(22.336642, 113.880412));
			points.Add(new Location(22.432119, 113.880408));
			points.Add(new Location(22.475107, 113.959233));
			points.Add(new Location(22.506303, 113.997266));

			Assert.True(LocationUtility.IsInPolygon(point, points));
		}
    }
}
