using System;
using System.Collections.Generic;

namespace Tiandao.Options.Profiles
{
    public class ProfileSectionCollection : Collections.NamedCollection<ProfileSection>
	{
		#region 构造方法

		public ProfileSectionCollection(ProfileItemCollection items) : base(items)
		{
		}

		#endregion

		#region 公共方法

		public ProfileSection Add(string name, int lineNumber = -1)
		{
			var item = new ProfileSection(name, lineNumber);
			base.Add(item);
			return item;
		}

		#endregion

		#region 重写方法

		protected override string GetKeyForItem(ProfileSection item)
		{
			return item.Name;
		}

		protected override bool OnItemMatch(object item)
		{
			return ((ProfileItem)item).ItemType == ProfileItemType.Section;
		}

		#endregion
	}
}
