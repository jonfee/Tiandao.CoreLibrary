using System;
using System.Reflection;

namespace Tiandao.Common
{
	/// <summary>
	/// 表示枚举项的描述。
	/// </summary>
	#if !CORE_CLR
	[Serializable]
	#endif
	public class EnumEntry : IFormattable, IFormatProvider
	{
		#region 私有字段

		private Type _type;
		private string _name;
		private object _value;
		private string _alias;
		private string _description;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取枚举项的名称。
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
		}

		/// <summary>
		/// 获取枚举的类型。
		/// </summary>
		public Type Type
		{
			get
			{
				return _type;
			}
		}

		/// <summary>
		/// 当前描述的枚举项值，该值有可能为枚举项的值也可能是对应的基元类型值。
		/// </summary>
		public object Value
		{
			get
			{
				return _value;
			}
		}

		/// <summary>
		/// 获取枚举项的别名，如果未定义建议创建者设置为枚举项的名称。
		/// </summary>
		/// <remarks>枚举项的别名由<seealso cref="ComponentModel.AliasAttribute"/>自定义特性指定。</remarks>
		public string Alias
		{
			get
			{
				return _alias;
			}
		}

		/// <summary>
		/// 当前描述枚举项的描述文本，如果未定义建议创建者设置为枚举项的名称。
		/// </summary>
		/// <remarks>枚举项的描述由<seealso cref="ComponentModel.DescriptionAttribute"/>自定义特性指定。</remarks>
		public string Description
		{
			get
			{
				return _description;
			}
		}

		#endregion

		#region 构造方法

		public EnumEntry(Type type, string name, object value, string alias, string description)
		{
			_type = type;
			_name = name;
			_value = value;
			_alias = alias ?? string.Empty;
			_description = description ?? string.Empty;
		}

		#endregion

		#region 重写方法

		public override string ToString()
		{
			string value;

			if(_value.GetType().IsPrimitive())
			{
				value = _value.ToString();
			}
			else
			{
				var field = _type.GetField(_name);

				value = Convert.ChangeType(field.GetValue(null), Enum.GetUnderlyingType(_type)).ToString();
			}

			return string.Format("{0}.{1} = {2}", _type.FullName, _name, value);
		}

		#endregion

		#region 格式方法

		public string ToString(string format)
		{
			if(string.IsNullOrWhiteSpace(format))
			{
				return _value.ToString();
			}

			switch(format.Trim().ToLowerInvariant())
			{
				case "d":
				case "description":
					return _description;
				case "n":
				case "name":
					return _name;
				case "a":
				case "alias":
					return _alias;
				case "f":
				case "full":
				case "fullname":
					return this.ToString();
			}

			return _value.ToString();
		}

		string IFormattable.ToString(string format, IFormatProvider formatProvider)
		{
			return this.ToString(format);
		}

		object IFormatProvider.GetFormat(Type formatType)
		{
			if(formatType == typeof(ICustomFormatter))
			{
				return this;
			}

			return null;
		}

		#endregion
	}
}