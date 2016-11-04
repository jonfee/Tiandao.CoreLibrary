using System;
using System.Collections.Generic;

namespace Tiandao.Options.Profiles
{
    public class ProfileItemCollection : Collections.Collection<ProfileItem>
	{
		#region 私有字段

		private object _owner;

		#endregion

		#region 内部属性

		internal object Owner
		{
			get
			{
				return _owner;
			}
			set
			{
				if(object.ReferenceEquals(_owner, value))
					return;

				foreach(var item in this.Items)
					item.Owner = value;
			}
		}

		#endregion

		#region 构造方法

		public ProfileItemCollection(object owner)
		{
			if(owner == null)
				throw new ArgumentNullException("owner");

			_owner = owner;
		}

		#endregion

		#region 重写方法

		protected override void InsertItems(int index, IEnumerable<ProfileItem> items)
		{
			foreach(var item in items)
			{
				item.Owner = _owner;
			}

			base.InsertItems(index, items);
		}

		protected override void SetItem(int index, ProfileItem item)
		{
			var oldItem = this.Items[index];

			if(oldItem != null)
				oldItem.Owner = null;

			item.Owner = _owner;

			base.SetItem(index, item);
		}

		protected override void RemoveItem(int index)
		{
			var item = this.Items[index];

			if(item != null)
				item.Owner = null;

			base.RemoveItem(index);
		}

		protected override void ClearItems()
		{
			foreach(var item in this.Items)
			{
				if(item != null)
					item.Owner = null;
			}

			base.ClearItems();
		}

		#endregion
	}
}
