using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tiandao.Options.Configuration
{
    public class OptionConfigurationPropertyCollection : KeyedCollection<string, OptionConfigurationProperty>
	{
		#region 构造方法

		public OptionConfigurationPropertyCollection() : base(StringComparer.OrdinalIgnoreCase)
		{

		}

		#endregion

		#region 保护方法

		protected override string GetKeyForItem(OptionConfigurationProperty item)
		{
			return item.Name;
		}

		#endregion

		#region 公共方法

		public bool TryGetValue(string name, out OptionConfigurationProperty value)
		{
			value = null;

			if(name == null)
				name = string.Empty;

			if(Contains(name))
			{
				value = this[name];
				return true;
			}

			return false;
		}

		#endregion
	}
}