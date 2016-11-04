using System;
using System.Collections.Generic;
using System.Linq;

namespace Tiandao.Options.Configuration
{
    public class OptionConfigurationSectionCollection : Collections.NamedCollectionBase<OptionConfigurationSection>
	{
		#region 构造方法

		public OptionConfigurationSectionCollection() : base(StringComparer.OrdinalIgnoreCase)
		{

		}

		#endregion

		#region 重写方法

		protected override string GetKeyForItem(OptionConfigurationSection item)
		{
			return item.Path;
		}

		#endregion

		#region 公共方法

		public OptionConfigurationSection Add(string path)
		{
			var section = new OptionConfigurationSection(path);
			this.Add(section);
			return section;
		}

		#endregion
	}
}