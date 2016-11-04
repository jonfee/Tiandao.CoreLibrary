using System;
using System.Collections.Generic;

namespace Tiandao.Options.Configuration
{
    public class SettingElement : OptionConfigurationElement
	{
		#region 常量定义

		private const string XML_NAME_ATTRIBUTE = "name";
		private const string XML_VALUE_ATTRIBUTE = "value";

		#endregion

		#region 公共属性

		[OptionConfigurationProperty(XML_NAME_ATTRIBUTE)]
		public string Name
		{
			get
			{
				return (string)this[XML_NAME_ATTRIBUTE];
			}
			set
			{
				if(string.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException();

				this[XML_NAME_ATTRIBUTE] = value.Trim();
			}
		}

		[OptionConfigurationProperty(XML_VALUE_ATTRIBUTE)]
		public string Value
		{
			get
			{
				return (string)this[XML_VALUE_ATTRIBUTE];
			}
			set
			{
				this[XML_VALUE_ATTRIBUTE] = value;
			}
		}

		#endregion

		#region 构造方法

		internal SettingElement()
		{

		}

		internal SettingElement(string name, string value)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));

			this.Name = name;
			this.Value = value;
		}

		#endregion
	}
}