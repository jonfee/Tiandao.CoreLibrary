using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Tiandao.Common;

namespace Tiandao.LBS
{
    public static class LocationUtility
    {
		#region 常量定义
		
	    private const double EARTH_RADIUS = 6378.137;   //地球半径

		#endregion

		#region 静态字段

		private static readonly Regex _regex = new Regex(@"(?<lat>\d+(\.\d+)?)\s*\,\s*(?<lng>\d+(\.\d+)?)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

		#endregion

		#region 公共方法

		/// <summary>
		/// 将一个以“,”分割的经纬度(纬度在前)字符串解析为一个地理坐标实例。
		/// </summary>
		/// <param name="text">字符串实例。</param>
		/// <returns>坐标实例。</returns>
		public static Location Parse(string text)
		{
			if(string.IsNullOrWhiteSpace(text))
				return null;

			var match = _regex.Match(text);

			if(match.Success)
			{
				//纬度值。
				var latitude = match.Groups["lat"].Value;

				//经度值。
				var longitude = match.Groups["lng"].Value;

				return new Location(latitude.ToDouble(), longitude.ToDouble());
			}

			return null;
		}

		/// <summary>
		/// 计算两个位置之间的直接距离，单位：公里。
		/// </summary>
		/// <param name="location1">位置一。</param>
		/// <param name="location2">位置二。</param>
		/// <returns>计算后的距离(公里)。</returns>
		public static double GetDistance(Location location1, Location location2)
	    {
			if(location1 == null)
				throw new ArgumentNullException("location1");

			if(location2 == null)
				throw new ArgumentNullException("location2");

		    return GetDistance(location1.Latitude, location1.Longitude, location2.Latitude, location2.Longitude);
	    }

		/// <summary>
		/// 计算两个位置之间的直接距离，单位：公里。
		/// </summary>
		/// <param name="latitude1">位置1的纬度值。</param>
		/// <param name="longitude1">位置1的经度值。</param>
		/// <param name="latitude2">位置2的纬度值。</param>
		/// <param name="longitude2">位置2的经度值。</param>
		/// <returns>计算后的距离(公里)。</returns>
		public static double GetDistance(double latitude1, double longitude1, double latitude2, double longitude2)
		{
			latitude1 = ToRadians(latitude1);
			longitude1 = ToRadians(longitude1);

			latitude2 = ToRadians(latitude2);
			longitude2 = ToRadians(longitude2);

			var latDis = latitude1 - latitude2;
			var lonDis = longitude1 - longitude2;

			var result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(latDis / 2), 2) + Math.Cos(latitude1) * Math.Cos(latitude2) * Math.Pow(Math.Sin(lonDis / 2), 2)));

			return Math.Round(result * EARTH_RADIUS * 10000) / 10000;
		}

		/// <summary>
		/// 以一个坐标点为中心计算出四个顶点。
		/// </summary>
		/// <param name="location">坐标点。</param> 
		/// <param name="distance">半径(公里)</param>
		/// <returns>指定半径外的四个顶点。</returns>
		public static Position GetPosition(Location location, double distance)
	    {
			if(location == null)
				throw new ArgumentNullException("location");

			return GetPosition(location.Latitude, location.Longitude, distance);
	    }

		/// <summary>
		/// 以一个坐标点为中心计算出周边四个顶点。
		/// </summary>
		/// <param name="latitude">纬度值。</param>
		/// <param name="longitude">经度值。</param>
		/// <param name="distance">半径(公里)</param>
		/// <returns>指定半径外的四个顶点。</returns>
		public static Position GetPosition(double latitude, double longitude, double distance)
	    {
		    var longitude1 = 2 * Math.Asin(Math.Sin(distance / (2 * EARTH_RADIUS)) / Math.Cos(ToRadians(latitude)));
		    longitude1 = ToDegrees(longitude1);

		    var latitude1 = distance / EARTH_RADIUS;
		    latitude1 = ToDegrees(latitude1);

		    return new Position
			(
				new Location(latitude + latitude1, longitude - longitude1), //left-top
			    new Location(latitude - latitude1, longitude - longitude1), //left-bottom
			    new Location(latitude + latitude1, longitude + longitude1), //right-top
			    new Location(latitude - latitude1, longitude + longitude1)  //right-bottom
			);
		}

		/// <summary>
		/// 监测指定的位置点 <paramref name="source"/> 是否在位置 <paramref name="target"/> 的指定公里范围内。
		/// </summary>
		/// <param name="source">位置点。</param>
		/// <param name="target">目标位置点。</param>
		/// <param name="distance">目标范围(公里)</param>
		/// <returns>如果在指定的范围内则为true，否则为false。</returns>
		public static bool IsInRange(Location source, Location target, double distance)
	    {
			if(source == null)
				throw new ArgumentNullException("source");

			if(target == null)
				throw new ArgumentNullException("target");

			// 计算目标位置的四个顶点。
			var position = LocationUtility.GetPosition(target, distance);

			return source.Latitude < position.LeftTop.Latitude && source.Latitude > position.RightBottom.Latitude && 
				   source.Longitude > position.LeftTop.Longitude && source.Longitude < position.RightBottom.Longitude;
	    }

		/// <summary>
		/// 监测指定的位置点 <paramref name="source"/> 是否在多边形顶点 <paramref name="points"/> 范围内。
		/// </summary>
		/// <param name="source">位置点。</param>
		/// <param name="points">多边形顶点集合。</param>
		/// <returns>如果在指定的多边形顶点范围内则为true，否则为false。</returns>
		public static bool IsInPolygon(Location source, IList<Location> points)
	    {
		    int count = points.Count;
			bool boundOrVertex = true;			//如果点位于多边形的顶点或边上，也算做点在多边形内，直接返回true
			int intersectCount = 0;				//cross points count of x 
			double precision = 2e-10;           //浮点类型计算时候与0比较时候的容差
			Location p1, p2;					//neighbour bound vertices
			Location p = source;                 //当前点
			p1 = points[0];                     //left vertex

		    for(var i = 1; i <= count; ++i)
		    {
				if(p.Equals(p1))
				{
					return boundOrVertex;//p is an vertex
				}

				p2 = points[i % count];

				// ray is outside of our interests
				if(p.Latitude < Math.Min(p1.Latitude, p2.Latitude) || p.Latitude > Math.Max(p1.Latitude, p2.Latitude))
				{
					p1 = p2;

					continue;//next ray left point
				}

				//ray is crossing over by the algorithm (common part of)
			    if(p.Latitude > Math.Min(p1.Latitude, p2.Latitude) && p.Latitude < Math.Max(p1.Latitude, p2.Latitude))
			    {
					//lng is before of ray
				    if(p.Longitude <= Math.Max(p1.Longitude, p2.Longitude))
				    {
						//overlies on a horizontal ray
						if(p1.Latitude == p2.Latitude && p.Longitude >= Math.Min(p1.Longitude, p2.Longitude))
						{
							return boundOrVertex;
						}

						if(p1.Longitude == p2.Longitude)
						{
							//ray is vertical
							if(p1.Longitude == p.Longitude)
							{
								//overlies on a vertical ray
								return boundOrVertex;
							}
							else
							{
								//before ray
								++intersectCount;
							}
						}
						else
						{
							//cross point on the left side
							var xinters = (p.Latitude - p1.Latitude) * (p2.Longitude - p1.Longitude) / (p2.Latitude - p1.Latitude) + p1.Longitude;

							//overlies on a ray
							if(Math.Abs(p.Longitude - xinters) < precision)
							{
								return boundOrVertex;
							}

							//before ray
							if(p.Longitude < xinters)
							{
								++intersectCount;
							}
						}
					}
			    }
			    else
			    {
					// special case when ray is crossing through the vertex
					if(p.Latitude == p2.Latitude && p.Longitude <= p2.Longitude)
					{
						// p crossing over p2
						var p3 = points[(i + 1) % count]; //next vertex

						// p.lat lies between p1.lat & p3.lat
						if(p.Latitude >= Math.Min(p1.Latitude, p3.Latitude) && p.Latitude <= Math.Max(p1.Latitude, p3.Latitude))
						{
							++intersectCount;
						}
						else
						{
							intersectCount += 2;
						}
					}
				}

				// next ray left point
				p1 = p2;
			}

			// 偶数在多边形外
			if(intersectCount % 2 == 0)
			{
				return false;
			}
			else
			{
				// 奇数在多边形内
				return true;
			}
	    }

	    #endregion

		#region 私有方法

		/// <summary>
		/// 角度数转换为弧度公式。
		/// </summary>
		/// <param name="number">角度数。</param>
		/// <returns>弧度。</returns>
		private static double ToRadians(double number)
		{
			return number * (Math.PI / 180.0);
		}

		/// <summary>
		/// 弧度转换为角度数公式。
		/// </summary>
		/// <param name="number">弧度。</param>
		/// <returns>角度数。</returns>
		private static double ToDegrees(double number)
		{
			return number * (180.0 / Math.PI);
		}

		#endregion
	}
}