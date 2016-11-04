using System;
using System.Collections.Generic;

namespace Tiandao.Common
{
	/// <summary>
	/// 提供一系列实用方法用于扩展 DateTime 类。
	/// </summary>
	public static class DateTimeExtension
    {
		#region 格式化

		/// <summary>
		/// 格式化日期时间。
		/// </summary>
		/// <param name="dateTime">日期时间实例。</param>
		/// <param name="mode">显示模式。</param>
		/// <returns>格式化后的字符串。</returns>
		public static string Format(this DateTime dateTime, int mode)
		{
			switch(mode)
			{
				case 0:
					return dateTime.ToString("yyyy-MM-dd");
				case 1:
					return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				case 2:
					return dateTime.ToString("yyyy/MM/dd");
				case 3:
					return dateTime.ToString("yyyy年MM月dd日");
				case 4:
					return dateTime.ToString("MM-dd");
				case 5:
					return dateTime.ToString("MM/dd");
				case 6:
					return dateTime.ToString("MM月dd日");
				case 7:
					return dateTime.ToString("yyyy-MM");
				case 8:
					return dateTime.ToString("yyyy/MM");
				case 9:
					return dateTime.ToString("yyyy年MM月");
				case 10:
					return dateTime.ToString("HH:mm:ss");
				default:
					return dateTime.ToString();
			}
		}

		#endregion
	}
}
