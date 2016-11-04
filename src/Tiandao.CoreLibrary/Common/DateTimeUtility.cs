using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Tiandao.Common
{
	/// <summary>
	/// 处理日期与时间的实用工具类。
	/// </summary>
	public static class DateTimeUtility
    {
		#region 随机日期

		/// <summary>
		/// 生成一个随机日期。
		/// </summary>
		/// <param name="dateTime1">起始日期。</param>
		/// <param name="dateTime2">结束日期。</param>
		/// <returns>日期实例。</returns>
		public static DateTime Random(DateTime dateTime1, DateTime dateTime2)
		{
			var random = new Random();
			var minTime = new DateTime();
			var maxTime = new DateTime();
			var timeSpan = new TimeSpan(dateTime1.Ticks - dateTime2.Ticks);

			double dTotalSecontds = timeSpan.TotalSeconds;
			double iTotalSecontds = 0;

			if(dTotalSecontds > int.MaxValue)
			{
				iTotalSecontds = int.MaxValue;
			}
			else if(dTotalSecontds < int.MinValue)
			{
				iTotalSecontds = int.MinValue;
			}
			else
			{
				iTotalSecontds = (int)dTotalSecontds;
			}

			if(iTotalSecontds > 0)
			{
				minTime = dateTime2;
				maxTime = dateTime1;
			}
			else if(iTotalSecontds < 0)
			{
				minTime = dateTime1;
				maxTime = dateTime2;
			}
			else
			{
				return dateTime1;
			}

			var maxValue = (int)iTotalSecontds;

			if(iTotalSecontds <= int.MinValue)
			{
				maxValue = int.MinValue + 1;
			}

			var i = random.Next(Math.Abs(maxValue));

			return minTime.AddSeconds(i);
		}

		#endregion

		#region 判断函数

		/// <summary>
		/// 判断指定的字符串值是否为一个有效的日期格式。
		/// </summary>
		/// <param name="value">需要检测的字符串值。</param>
		/// <returns>检测结果。</returns>
		public static bool IsDateTime(string value)
		{
			if(string.IsNullOrEmpty(value))
				return false;

			const string regexDate = @"[1-2]{1}[0-9]{3}((-|[.]){1}(([0]?[1-9]{1})|(1[0-2]{1}))((-|[.]){1}((([0]?[1-9]{1})|([1-2]{1}[0-9]{1})|(3[0-1]{1})))( (([0-1]{1}[0-9]{1})|2[0-3]{1}):([0-5]{1}[0-9]{1}):([0-5]{1}[0-9]{1})(\.[0-9]{3})?)?)?)?$";

			if(Regex.IsMatch(value, regexDate))
			{
				//以下各月份日期验证，保证验证的完整性   
				int indexY = -1;
				int indexM = -1;
				int indexD = -1;

				if(-1 != (indexY = value.IndexOf("-")))
				{
					indexM = value.IndexOf("-", indexY + 1);
					indexD = value.IndexOf(":");
				}
				else
				{
					indexY = value.IndexOf(".");
					indexM = value.IndexOf(".", indexY + 1);
					indexD = value.IndexOf(":");
				}

				//不包含日期部分，直接返回true   
				if(-1 == indexM)
					return true;

				if(-1 == indexD)
					indexD = value.Length + 3;

				int iYear = Convert.ToInt32(value.Substring(0, indexY));
				int iMonth = Convert.ToInt32(value.Substring(indexY + 1, indexM - indexY - 1));
				int iDate = Convert.ToInt32(value.Substring(indexM + 1, indexD - indexM - 4));

				//判断月份日期   
				if((iMonth < 8 && 1 == iMonth % 2) || (iMonth > 8 && 0 == iMonth % 2))
				{
					if(iDate < 32)
					{
						return true;
					}
				}
				else
				{
					if(iMonth != 2)
					{
						if(iDate < 31)
						{
							return true;
						}
					}
					else
					{
						//闰年   
						if((0 == iYear % 400) || (0 == iYear % 4 && 0 < iYear % 100))
						{
							if(iDate < 30)
							{
								return true;
							}
						}
						else
						{
							if(iDate < 29)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}

		/// <summary>
		/// 判断指定的年份是否为闰年。
		/// </summary>
		/// <param name="year">年份。</param>
		/// <returns>检测结果。</returns>
		public static bool IsLeapYear(int year)
		{
			return (year % 400 == 0) || (year % 4 == 0 && year % 100 != 0);
		}

		#endregion

		#region 日期比较

		/// <summary>
		/// 获取两个日期时间的间隔。
		/// </summary>
		/// <param name="dateTime1">第一个日期时间实例。</param>
		/// <param name="dateTime2">第二个日期时间实例。</param>
		/// <returns>时间间隔。</returns>
		public static TimeSpan DateDiff(DateTime dateTime1, DateTime dateTime2)
		{
			TimeSpan timeSpan1 = new TimeSpan(dateTime1.Ticks);
			TimeSpan timeSpan2 = new TimeSpan(dateTime2.Ticks);

			return timeSpan1.Subtract(timeSpan2).Duration();
		}

		/// <summary>
		/// 获取两个日期的时间差描述信息。
		/// </summary>
		/// <param name="dateTime1">第一个日期时间实例。</param>
		/// <param name="dateTime2">第二个日期时间实例。</param>
		/// <returns>描述信息。</returns>
		public static string DateDiff2(DateTime dateTime1, DateTime dateTime2)
		{
			string result;

			TimeSpan timeSpan = dateTime2 - dateTime1;

			if(timeSpan.Days >= 1)
			{
				result = dateTime1.Day + "天前";
			}
			else
			{
				if(timeSpan.Hours > 1)
				{
					result = timeSpan.Hours + "小时前";
				}
				else
				{
					result = timeSpan.Minutes + "分钟前";
				}
			}

			return result;
		}

		#endregion

		#region 日期转换

		/// <summary>
		/// 将秒数转换为分钟数。
		/// </summary>
		/// <param name="second">秒数。</param>
		/// <returns>分钟数。</returns>
		public static int ConvertToMinute(int second)
		{
			decimal minute = second / (decimal)60;

			return Convert.ToInt32(Math.Ceiling(minute));
		}

		#endregion

		#region 获取函数

		/// <summary>
		/// 获取指定日期月份中的第一天。
		/// </summary>
		/// <param name="year">年份。</param>
		/// <param name="month">月份。</param>
		/// <returns>第一天的日期实例。</returns>
		public static DateTime GetFirstDayOfMonth(int year, int month)
		{
			return new DateTime(year, month, 1);
		}

		/// <summary>
		/// 获取指定日期月份中的第一天。
		/// </summary>
		/// <param name="dateTime">日期实例。</param>
		/// <returns>第一天的日期实例。</returns>
		public static DateTime GetFirstDayOfMonth(DateTime dateTime)
		{
			return GetFirstDayOfMonth(dateTime.Year, dateTime.Month);
		}

		/// <summary>
		/// 获取指定日期月份中的最后一天。
		/// </summary>
		/// <param name="year">年份。</param>
		/// <param name="month">月份。</param>
		/// <returns>最后一天的日期实例。</returns>
		public static DateTime GetLastDayOfMonth(int year, int month)
		{
			return new DateTime(year, month, new GregorianCalendar().GetDaysInMonth(year, month));
		}

		/// <summary>
		/// 获取指定日期月份中的最后一天。
		/// </summary>
		/// <param name="dateTime">日期实例。</param>
		/// <returns>最后一天的日期实例。</returns>
		public static DateTime GetLastDayOfMonth(DateTime dateTime)
		{
			return GetLastDayOfMonth(dateTime.Year, dateTime.Month);
		}

		/// <summary>
		/// 获取指定日期季度中的第一天。
		/// </summary>
		/// <param name="year">年份。</param>
		/// <param name="quarter">季度。</param>
		/// <returns>第一天的日期实例。</returns>
		public static DateTime GetFirstDayOfQuarter(int year, int quarter)
		{
			var month = 0;

			switch(quarter)
			{
				case 1:
					month = 1;
					break;
				case 2:
					month = 4;
					break;
				case 3:
					month = 7;
					break;
				case 4:
					month = 11;
					break;
				default:
					throw new ArgumentException("请指定有效的季度。");
			}

			return new DateTime(year, month, 1);
		}

		/// <summary>
		/// 获取指定日期季度中的第一天。
		/// </summary>
		/// <param name="dateTime">日期实例。</param>
		/// <returns>第一天的日期实例。</returns>
		public static DateTime GetFirstDayOfQuarter(DateTime dateTime)
		{
			var month = 0;

			switch(dateTime.Month)
			{
				case 1:
				case 2:
				case 3:
					month = 1;
					break;
				case 4:
				case 5:
				case 6:
					month = 4;
					break;
				case 7:
				case 8:
				case 9:
					month = 7;
					break;
				case 10:
				case 11:
				case 12:
					month = 11;
					break;
			}

			return new DateTime(dateTime.Year, month, 1);
		}

		/// <summary>
		/// 获取指定日期季度中的最后一天。
		/// </summary>
		/// <param name="year">年份。</param>
		/// <param name="quarter">季度。</param>
		/// <returns>最后一天的日期实例。</returns>
		public static DateTime GetLastDayOfQuarter(int year, int quarter)
		{
			return GetFirstDayOfQuarter(year, quarter).AddMonths(3).AddDays(-1);
		}

		/// <summary>
		/// 获取指定日期季度中的最后一天。
		/// </summary>
		/// <param name="dateTime">日期实例。</param>
		/// <returns>最后一天的日期实例。</returns>
		public static DateTime GetLastDayOfQuarter(DateTime dateTime)
		{
			return GetFirstDayOfQuarter(dateTime).AddMonths(3).AddDays(-1);
		}

		/// <summary>
		/// 获取指定日期年份中的第一天。
		/// </summary>
		/// <param name="year">年份。</param>
		/// <returns>第一天的日期实例。</returns>
		public static DateTime GetFirstDayOfYear(int year)
		{
			return new DateTime(year, 1, 1);
		}

		/// <summary>
		/// 获取指定日期年份中的第一天。
		/// </summary>
		/// <param name="dateTime">日期实例。</param>
		/// <returns>第一天的日期实例。</returns>
		public static DateTime GetFirstDayOfYear(DateTime dateTime)
		{
			return GetFirstDayOfYear(dateTime.Year);
		}

		/// <summary>
		/// 获取指定日期年份中的最后一天。
		/// </summary>
		/// <param name="year">年份。</param>
		/// <returns>最后一天的日期实例。</returns>
		public static DateTime GetLastDayOfYear(int year)
		{
			return new DateTime(year, 12, 31);
		}

		/// <summary>
		/// 获取指定日期年份中的最后一天。
		/// </summary>
		/// <param name="dateTime">日期实例。</param>
		/// <returns>最后一天的日期实例。</returns>
		public static DateTime GetLastDayOfYear(DateTime dateTime)
		{
			return GetLastDayOfYear(dateTime.Year);
		}

		/// <summary>
		/// 返回一个字符串，表示星期中指定的某一天。
		/// </summary>
		/// <param name="year">年份。</param>
		/// <param name="month">月份。</param>
		/// <param name="day">天数。</param>
		/// <returns>字符串值。</returns>
		public static string GetWeekDayName(int year, int month, int day)
		{
			string[] weekDays =
			{
				"日", "一", "二", "三", "四", "五", "六"
			};

			return string.Format("星期{0}", weekDays[GetWeekDay(year, month, day)]);
		}

		/// <summary>
		/// 返回一个字符串，表示星期中指定的某一天。
		/// </summary>
		/// <param name="dateTime">日期实例。</param>
		/// <returns>字符串值。</returns>
		public static string GetWeekDayName(DateTime dateTime)
		{
			return GetWeekDayName(dateTime.Year, dateTime.Month, dateTime.Day);
		}

		/// <summary>
		/// 根据给定的年月日，计算出对应的星期数。
		/// </summary>
		/// <param name="year">年份。</param>
		/// <param name="month">月份。</param>
		/// <param name="day">天数。</param>
		/// <returns>星期数。</returns>
		public static int GetWeekDay(int year, int month, int day)
		{
			if(month < 3)
			{
				month += 12;
				year--;
			}

			if(year % 400 == 0 || year % 100 != 0 && year % 4 == 0)
			{
				day--;
			}

			else
			{
				day += 1;
			}

			return (day + 2 * month + 3 * (month + 1) / 5 + year + year / 4 - year / 100 + year / 400) % 7;
		}

		/// <summary>
		/// 根据给定的年月日，计算出对应的星期数。
		/// </summary>
		/// <param name="dateTime">日期实例。</param>
		/// <returns>星期数。</returns>
		public static int GetWeekDay(DateTime dateTime)
		{
			return GetWeekDay(dateTime.Year, dateTime.Month, dateTime.Day);
		}

		/// <summary>
		/// 返回指定年份中的天数。
		/// </summary>
		/// <param name="year">年份。</param>
		/// <returns>天数。</returns>
		public static int GetDaysOfYear(int year)
		{
			return IsLeapYear(year) ? 366 : 365;
		}

		/// <summary>
		/// 返回指定日期年份中的天数。
		/// </summary>
		/// <param name="dateTime">日期实例。</param>
		/// <returns>天数。</returns>
		public static int GetDaysOfYear(DateTime dateTime)
		{
			return GetDaysOfYear(dateTime.Year);
		}

		/// <summary>
		/// 返回指定月份中的天数。
		/// </summary>
		/// <param name="year">年份。</param>
		/// <param name="month">月份。</param>
		/// <returns>天数。</returns>
		public static int GetDaysOfMonth(int year, int month)
		{
			switch(month)
			{
				case 1:
				case 3:
				case 5:
				case 7:
				case 8:
				case 10:
				case 12:
					return 31;
				case 4:
				case 6:
				case 9:
				case 11:
					return 30;
				default:
					return IsLeapYear(year) ? 29 : 28;
			}
		}

		/// <summary>
		/// 返回指定日期月份中的天数。
		/// </summary>
		/// <param name="dateTime">日期实例。</param>
		/// <returns>天数。</returns>
		public static int GetDaysOfMonth(DateTime dateTime)
		{
			return GetDaysOfMonth(dateTime.Year, dateTime.Month);
		}

		/// <summary>
		/// 获取某一年有多少周。
		/// </summary>
		/// <param name="year">年份</param>
		/// <returns>该年周数。</returns>
		public static int GetWeekAmount(int year)
		{
			//该年最后一天
			var end = new DateTime(year, 12, 31);

			var gc = new GregorianCalendar();

			//该年星期数
			return gc.GetWeekOfYear(end, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
		}

		/// <summary>
		/// 获取某一日期是该年中的第几周。
		/// </summary>
		/// <param name="dateTime">日期实例。</param>
		/// <returns>该日期在该年中的周数。</returns>
		public static int GetWeekOfYear(DateTime dateTime)
		{
			var gc = new GregorianCalendar();

			return gc.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
		}

		/// <summary>
		/// 根据某年的第几周获取这周的起止日期。
		/// </summary>
		/// <param name="year">年份。</param>
		/// <param name="week">周数。</param>
		/// <param name="firstDate">起始日期。</param>
		/// <param name="lastDate">结束日期。</param>
		public static void GetWeekRange(int year, int week, ref DateTime firstDate, ref DateTime lastDate)
		{
			//当年的第一天
			var firstDay = new DateTime(year, 1, 1);

			//当年的第一天是星期几
			int firstOfWeek = Convert.ToInt32(firstDay.DayOfWeek);

			//计算当年第一周的起止日期，可能跨年
			int dayDiff = (-1) * firstOfWeek + 1;
			int dayAdd = 7 - firstOfWeek;

			firstDate = firstDay.AddDays(dayDiff).Date;
			lastDate = firstDay.AddDays(dayAdd).Date;

			//如果不是要求计算第一周
			if(week != 1)
			{
				int addDays = (week - 1) * 7;
				firstDate = firstDate.AddDays(addDays);
				lastDate = lastDate.AddDays(addDays);
			}
		}

		/// <summary>
		/// 返回两个日期之间相差的天数。
		/// </summary>
		/// <param name="from">起始日期。</param>
		/// <param name="to">结束日期。</param>
		/// <returns>天数</returns>
		public static int GetDiffDays(DateTime from, DateTime to)
		{
			TimeSpan timeSpan = from.Date - to.Date;

			return timeSpan.Days;
		}

		#endregion
	}
}
