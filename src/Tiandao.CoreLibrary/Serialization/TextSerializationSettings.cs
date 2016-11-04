using System;
using System.Collections.Generic;
using System.Text;

namespace Tiandao.Serialization
{
    public class TextSerializationSettings : SerializationSettings
	{
		#region 私有字段

		private bool _indented;
		private bool _typed;
		private DateFormatHandling _dateFormatHandling;
		private string _dateFormatString;
		private SerializationNamingConvention _namingConvention;

		#endregion

		#region 静态字段

		public static TextSerializationSettings Default = new TextSerializationSettings();

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取或设置一个值，指示序列化后的文本是否保持缩进风格。
		/// </summary>
		public bool Indented
		{
			get
			{
				return _indented;
			}
			set
			{
				this.SetPropertyValue(() => this.Indented, ref _indented, value);
			}
		}

		/// <summary>
		/// 获取或设置一个值，指示序列化的文本是否保持强类型信息。
		/// </summary>
		public bool Typed
		{
			get
			{
				return _typed;
			}
			set
			{
				this.SetPropertyValue(() => this.Typed, ref _typed, value);
			}
		}

		/// <summary>
		/// 获取或设置一个值，指示序列化的时间格式化处理类型。
		/// </summary>
		public DateFormatHandling DateFormatHandling
		{
			get
			{
				return _dateFormatHandling;
			}
			set
			{
				this.SetPropertyValue(() => this.DateFormatHandling, ref _dateFormatHandling, value);
			}
		}

		/// <summary>
		/// 获取或设置一个值，指示序列化的时间格式化字符串。
		/// </summary>
		public string DateFormatString
		{
			get
			{
				return _dateFormatString;
			}
			set
			{
				this.SetPropertyValue(() => this.DateFormatString, ref _dateFormatString, value);
			}
		}

		/// <summary>
		/// 获取或设置一个值，指示序列化成员的命名转换方式。
		/// </summary>
		public SerializationNamingConvention NamingConvention
		{
			get
			{
				return _namingConvention;
			}
			set
			{
				this.SetPropertyValue(() => this.NamingConvention, ref _namingConvention, value);
			}
		}

		#endregion

		#region 构造方法

		public TextSerializationSettings()
		{
			_indented = false;
			_dateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
			_dateFormatString = DateFormatStrings.FULL;
			_namingConvention = SerializationNamingConvention.None;
		}

		#endregion
	}
}
