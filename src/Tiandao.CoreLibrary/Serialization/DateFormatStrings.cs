using System;
using System.Linq;

namespace Tiandao.Serialization
{
	/// <summary>
	/// 日期与时间格式化模板类。
	/// </summary>
    public sealed class DateFormatStrings
    {
		/// <summary>
		/// 完整的日期与时间格式，例：2016-10-25 17:52:29
		/// </summary>
		public const string FULL = "yyyy-MM-dd HH:mm:ss";

		/// <summary>
		/// 仅带日期的格式，例：2016-10-25
		/// </summary>
		public const string DATE = "yyyy-MM-dd";

		/// <summary>
		/// 仅带时间的格式，例：17:52:29
		/// </summary>
		public const string TIME = "HH:mm:ss";
    }
}