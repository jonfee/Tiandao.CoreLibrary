using System;

using Tiandao.Common;

namespace Tiandao.LBS
{
	/// <summary>
	/// 表示一个地理坐标点。
	/// </summary>
#if !CORE_CLR
	[Serializable]
#endif
	public class Location
	{
		#region 公共属性

		/// <summary>
		/// 获取或设置纬度值。
		/// </summary>
		public double Latitude
		{
			get;
			set;
		}

		/// <summary>
		/// 获取或设置经度值。
		/// </summary>
		public double Longitude
	    {
		    get;
		    set;
	    }

		/// <summary>
		/// 获取当前坐标所属坐标系。
		/// </summary>
		public LocationSeries Series
		{
			get;
			private set;
		}

		#endregion

		#region 构造方法

		/// <summary>
		/// 初始化 Location 类的新实例。
		/// </summary>
		/// <param name="latitude">纬度值。</param>
		/// <param name="longitude">经度值</param>
		/// <param name="series">坐标系。</param>
		public Location(double latitude, double longitude, LocationSeries series = LocationSeries.Unknown)
	    {
//			if(Math.Abs(latitude) <= 0 || Math.Abs(longitude) <= 0)
//				throw new ArgumentException("Invalid latitude or longitude.");

			if(Math.Abs(latitude) > 90 || Math.Abs(longitude) > 180)
				throw new ArgumentException("Latitude or longitude out of range.");

			this.Latitude = latitude;
			this.Longitude = longitude;
		    this.Series = series;
	    }

		/// <summary>
		/// 初始化 Location 类的新实例。
		/// </summary>
		/// <param name="latitude">纬度值。</param>
		/// <param name="longitude">经度值</param>
		/// <param name="series">坐标系。</param>
		public Location(string latitude, string longitude, LocationSeries series = LocationSeries.Unknown)
		{
			if(string.IsNullOrWhiteSpace(latitude))
				throw new ArgumentNullException("latitude");

			if(string.IsNullOrWhiteSpace(longitude))
				throw new ArgumentNullException("longitude");

			var latitude1 = latitude.ToDouble(0);
			var longitude1 = longitude.ToDouble(0);

//			if(Math.Abs(latitude1) <= 0 || Math.Abs(longitude1) <= 0)
//				throw new ArgumentException("Invalid latitude or longitude.");

			if(Math.Abs(latitude1) > 90 || Math.Abs(longitude1) > 180)
				throw new ArgumentException("Latitude or longitude out of range.");

			this.Latitude = latitude1;
			this.Longitude = longitude1;
			this.Series = series;
		}

		#endregion

		#region 重写方法

		public override bool Equals(object obj)
		{
			var location = obj as Location;

			if(location == null)
				return false;

			return this.Latitude.Equals(location.Latitude) && this.Longitude.Equals(location.Longitude);
		}

		public override string ToString()
		{
			return string.Format("{0},{1}", this.Latitude, this.Longitude);
		}

		#endregion
	}
}