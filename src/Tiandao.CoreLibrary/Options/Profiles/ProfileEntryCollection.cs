using System;
using System.Collections.Generic;

namespace Tiandao.Options.Profiles
{
    public class ProfileEntryCollection : Collections.NamedCollection<ProfileEntry>
	{
		#region 构造方法

		public ProfileEntryCollection(ProfileItemCollection items) : base(items)
		{

		}

		#endregion

		#region 公共方法

		public ProfileEntry Add(string name, string value = null)
		{
			var item = new ProfileEntry(name, value);
			base.Add(item);
			return item;
		}

		public ProfileEntry Add(int lineNumber, string name, string value = null)
		{
			var item = new ProfileEntry(lineNumber, name, value);
			base.Add(item);
			return item;
		}

		#endregion

		#region 重写方法

		protected override string GetKeyForItem(ProfileEntry item)
		{
			return item.Name;
		}

		protected override bool OnItemMatch(object item)
		{
			return ((ProfileItem)item).ItemType == ProfileItemType.Entry;
		}

		#endregion
	}
}
