using System;
using System.ComponentModel;
using System.Globalization;

namespace Tiandao.LBS
{
    public class LocationConverter : TypeConverter
	{
		#region 常量定义

		private const double PI = 3.14159265358979324;
		private const double X_PI = 3.14159265358979324 * 3000.0 / 180.0;
		private const double EE = 0.00669342162296594323;
		private const double AA = 6378245.0;

		#endregion

		#region 重写方法

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return LocationUtility.Parse(value as string);
		}

		#endregion

		#region 静态方法

		/// <summary>
		/// 将世界标准地理坐标(GPS:WGS-84)转换成中国国测局地理坐标(火星:GCJ-02)
		/// </summary>
		/// <remarks>只在中国大陆的范围的坐标有效，以外直接返回世界标准坐标。</remarks>
		/// <param name="location">GPS坐标实例。</param>
		/// <returns>GCJ-02坐标实例。</returns>
		public static Location Convert84To02(Location location)
		{
			return Convert84To02(location.Latitude, location.Longitude);
		}

		/// <summary>
		/// 将世界标准地理坐标(GPS:WGS-84)转换成中国国测局地理坐标(火星:GCJ-02)
		/// </summary>
		/// <param name="latitude">GPS纬度值。</param>
		/// <param name="longitude">GPS经度值。</param>
		/// <remarks>只在中国大陆的范围的坐标有效，以外直接返回世界标准坐标。</remarks>
		/// <returns>GCJ-02坐标实例。</returns>
		public static Location Convert84To02(double latitude, double longitude)
		{
			return EncryptGCJ02(latitude, longitude);
		}

		/// <summary>
		/// 将中国国测局地理坐标(火星:GCJ-02)转换成世界标准地理坐标(GPS:WGS-84)
		/// </summary>
		/// <remarks>此接口有1-2米左右的误差，需要精确的场景慎用。</remarks>
		/// <param name="location">GCJ-02坐标实例。</param>
		/// <returns>WGS-84坐标实例。</returns>
		public static Location Convert02To84(Location location)
		{
			return Convert02To84(location.Latitude, location.Longitude);
		}

		/// <summary>
		/// 将中国国测局地理坐标(火星:GCJ-02)转换成世界标准地理坐标(GPS:WGS-84)
		/// </summary>
		/// <remarks>此接口有1-2米左右的误差，需要精确的场景慎用。</remarks>
		/// <param name="latitude">GCJ-02纬度值。</param>
		/// <param name="longitude">GCJ-02经度值。</param>
		/// <returns>WGS-84坐标实例。</returns>
		public static Location Convert02To84(double latitude, double longitude)
		{
			return DecryptGCJ02(latitude, longitude);
		}

		/// <summary>
		/// 将世界标准地理坐标(GPS:WGS-84)转换成百度地理坐标(BD-09)
		/// </summary>
		/// <param name="location">GPS坐标实例。</param>
		/// <returns>BD-09坐标实例。</returns>
		public static Location Convert84To09(Location location)
		{
			return Convert84To09(location.Latitude, location.Longitude);
		}

		/// <summary>
		/// 将世界标准地理坐标(GPS:WGS-84)转换成百度地理坐标(BD-09)
		/// </summary>
		/// <param name="latitude">GPS纬度值。</param>
		/// <param name="longitude">GPS经度值。</param>
		/// <returns>BD-09坐标实例。</returns>
		public static Location Convert84To09(double latitude, double longitude)
		{
			//将WGS-84转换为GCJ-02
			var bufferLocation = EncryptGCJ02(latitude, longitude);

			//将GCJ-02转换BD-09坐标。
			return EncryptBD09(bufferLocation.Latitude, bufferLocation.Longitude);
		}

		/// <summary>
		/// 将百度地理坐标(BD-09)转换成世界标准地理坐标(GPS:WGS-84)
		/// </summary>
		/// <remarks>此接口有1－2米左右的误差，需要精确定位情景慎用</remarks>
		/// <param name="location">BD-09坐标实例。</param>
		/// <returns>WGS-84坐标实例。</returns>
		public static Location Convert09To84(Location location)
		{
			return Convert09To84(location.Latitude, location.Longitude);
		}

		/// <summary>
		/// 将百度地理坐标(BD-09)转换成世界标准地理坐标(GPS:WGS-84)
		/// </summary>
		/// <remarks>此接口有1－2米左右的误差，需要精确定位情景慎用</remarks>
		/// <param name="latitude">BD-09坐标纬度值。</param>
		/// <param name="longitude">BD-09坐标经度值。</param>
		/// <returns>WGS-84坐标实例。</returns>
		public static Location Convert09To84(double latitude, double longitude)
		{
			//将BD-09转换为GCJ-02
			var bufferLocation = Convert09To02(latitude, longitude);

			//将GCJ-02转换WGS-84坐标。
			return DecryptGCJ02(bufferLocation.Latitude, bufferLocation.Longitude);
		}

		/// <summary>
		/// 将百度地理坐标(BD-09)转换成中国国测局地理坐标(火星:GCJ-02)
		/// </summary>
		/// <param name="location">BD-09坐标实例。</param>
		/// <returns>GCJ-02坐标实例。</returns>
		public static Location Convert09To02(Location location)
		{
			return Convert09To02(location.Latitude, location.Longitude);
		}

		/// <summary>
		/// 将百度地理坐标(BD-09)转换成中国国测局地理坐标(火星:GCJ-02)
		/// </summary>
		/// <param name="latitude">BD-09纬度值。</param>
		/// <param name="longitude">BD-09经度值。</param>
		/// <returns>GCJ-02坐标实例。</returns>
		public static Location Convert09To02(double latitude, double longitude)
		{
			return DecryptBD09(latitude, longitude);
		}

		/// <summary>
		/// 将中国国测局地理坐标(火星:GCJ-02)转换成百度地理坐标(BD-09)
		/// </summary>
		/// <param name="location">GCJ-02坐标实例。</param>
		/// <returns>BD-09坐标实例。</returns>
		public static Location Convert02To09(Location location)
		{
			return Convert02To09(location.Latitude, location.Longitude);
		}

		/// <summary>
		/// 将中国国测局地理坐标(火星:GCJ-02)转换成百度地理坐标(BD-09)
		/// </summary>
		/// <param name="latitude">GCJ-02坐标纬度值。</param>
		/// <param name="longitude">GCJ-02坐标经度值。</param>
		/// <returns>BD-09坐标实例。</returns>
		public static Location Convert02To09(double latitude, double longitude)
		{
			return EncryptBD09(latitude, longitude);
		}

		#endregion

		#region 私有方法

		/// <summary>
		/// 将世界标准地理坐标(GPS:WGS-84)加密成中国国测局地理坐标(火星:GCJ-02)
		/// </summary>
		/// <remarks>只在中国大陆的范围的坐标有效，以外直接返回世界标准坐标。</remarks>
		/// <param name="latitude">GPS纬度。</param>
		/// <param name="longitude">GPS经度。</param>
		/// <returns>GCJ-02坐标实例。</returns>
		private static Location EncryptGCJ02(double latitude, double longitude)
		{
			//如果超出大陆的坐标范围，则直接返回世界标准坐标。
			if(IsOutOfChina(latitude, longitude))
				return new Location(latitude, longitude, LocationSeries.WGS84);

			//开始转换纬度值。
			double bufferLatitude = TransformLatitude(latitude - 35.0, longitude - 105.0);

			//开始转换经度值。
			double bufferLongitude = TransformLongitude(latitude - 35.0, longitude - 105.0);

			//获取弧度。
			double radLatitude = latitude / 180.0 * PI;
			double magic = Math.Sin(radLatitude);
			magic = 1 - EE * magic * magic;
			double sqrtMagic = Math.Sqrt(magic);
			bufferLongitude = (bufferLongitude * 180.0) / (AA / sqrtMagic * Math.Cos(radLatitude) * PI);
			bufferLatitude = (bufferLatitude * 180.0) / ((AA * (1 - EE)) / (magic * sqrtMagic) * PI);

			//返回结果。
			return new Location(latitude + bufferLatitude, longitude + bufferLongitude, LocationSeries.GCJ02);
		}

		/// <summary>
		/// 将中国国测局地理坐标(火星:GCJ-02)解密成世界标准地理坐标(GPS:WGS-84)
		/// </summary>
		/// <remarks>此接口有1-2米左右的误差，需要精确的场景慎用。</remarks>
		/// <param name="latitude">GPS纬度。</param>
		/// <param name="longitude">GPS经度。</param>
		/// <returns>WGS-84坐标实例。</returns>
		private static Location DecryptGCJ02(double latitude, double longitude)
		{
			var location = EncryptGCJ02(latitude, longitude);

			double latitude1 = location.Latitude - latitude;
			double longitude1 = location.Longitude - longitude;

			//返回结果。
			return new Location(latitude - latitude1, longitude - longitude1, LocationSeries.WGS84);
		}

		/// <summary>
		/// 将中国国测局地理坐标(火星:GCJ-02)加密成百度地理坐标(BD-09)
		/// </summary>
		/// <param name="latitude">GCJ-02纬度值。</param>
		/// <param name="longitude">GCJ-02经度值。</param>
		/// <returns>BD-09坐标实例。</returns>
		private static Location EncryptBD09(double latitude, double longitude)
		{
			double x = latitude + 0.006;
			double y = longitude + 0.0065;
			double z = Math.Sqrt(y * y + x * x) + 0.00002 * Math.Sin(x * X_PI);
			double theta = Math.Atan2(x, y) + 0.000003 * Math.Cos(y * X_PI);

			var latitude1 = z * Math.Sin(theta);
			var longitude1 = z * Math.Cos(theta);

			//返回结果。
			return new Location(latitude1, longitude1, LocationSeries.BD09);
		}

		/// <summary>
		/// 将百度地理坐标(BD-09)解密成中国国测局地理坐标(火星:GCJ-02)
		/// </summary>
		/// <param name="latitude">BD-09纬度值。</param>
		/// <param name="longitude">BD-09经度值。</param>
		/// <returns>GCJ-02坐标实例。</returns>
		private static Location DecryptBD09(double latitude, double longitude)
		{
			double x = latitude - 0.006;
			double y = longitude - 0.0065;
			double z = Math.Sqrt(y * y + x * x) - 0.00002 * Math.Sin(x * X_PI);
			double theta = Math.Atan2(x, y) - 0.000003 * Math.Cos(y * X_PI);

			var latitude1 = z * Math.Sin(theta);
			var longitude1 = z * Math.Cos(theta);

			//返回结果。
			return new Location(latitude1, longitude1, LocationSeries.GCJ02);
		}

		/// <summary>
		/// 判断指定的GPS经纬度是否超出中国大陆范围。
		/// </summary>
		/// <param name="latitude">纬度值。</param>
		/// <param name="longitude">经度值。</param>
		/// <returns>验证结果。</returns>
		private static bool IsOutOfChina(double latitude, double longitude)
		{
			if(latitude < 0.8293 || latitude > 55.8271)
				return true;

			if(longitude < 72.004 || longitude > 137.8347)
				return true;

			return false;
		}

		/// <summary>
		/// 转换GPS经度值。
		/// </summary>
		/// <param name="latitude">纬度值。</param>
		/// <param name="longitude">经度值。</param>
		/// <returns>转换后的经度值。</returns>
		private static double TransformLongitude(double latitude, double longitude)
		{
			var result = 300.0 + longitude + 2.0 * latitude + 0.1 * longitude * longitude + 0.1 * longitude * latitude + 0.1 * Math.Sqrt(Math.Abs(longitude));

			result += (20.0 * Math.Sin(6.0 * longitude * PI) + 20.0 * Math.Sin(2.0 * longitude * PI)) * 2.0 / 3.0;
			result += (20.0 * Math.Sin(longitude * PI) + 40.0 * Math.Sin(longitude / 3.0 * PI)) * 2.0 / 3.0;
			result += (150.0 * Math.Sin(longitude / 12.0 * PI) + 300.0 * Math.Sin(longitude / 30.0 * PI)) * 2.0 / 3.0;

			return result;
		}

		/// <summary>
		/// 转换GPS纬度值。
		/// </summary>
		/// <param name="latitude">纬度值。</param>
		/// <param name="longitude">经度值。</param>
		/// <returns>转换后的纬度值。</returns>
		private static double TransformLatitude(double latitude, double longitude)
		{
			var result = -100.0 + 2.0 * longitude + 3.0 * latitude + 0.2 * latitude * latitude + 0.1 * longitude * latitude + 0.2 * Math.Sqrt(Math.Abs(longitude));

			result += (20.0 * Math.Sin(6.0 * longitude * PI) + 20.0 * Math.Sin(2.0 * longitude * PI)) * 2.0 / 3.0;
			result += (20.0 * Math.Sin(latitude * PI) + 40.0 * Math.Sin(latitude / 3.0 * PI)) * 2.0 / 3.0;
			result += (160.0 * Math.Sin(latitude / 12.0 * PI) + 320 * Math.Sin(latitude * PI / 30.0)) * 2.0 / 3.0;

			return result;
		}

		#endregion
	}
}
