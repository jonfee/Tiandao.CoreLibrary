using System;
using System.Collections.Generic;

namespace Tiandao.Collections
{
    public class HierarchicalNodeCollection<T> : NamedCollectionBase<T> where T : HierarchicalNode
	{
		#region 私有字段

		private T _owner;

		#endregion

		#region 保护属性

		protected T Owner
		{
			get
			{
				return _owner;
			}
		}

		#endregion

		#region 构造方法

		protected HierarchicalNodeCollection(T owner)
		{
			_owner = owner;
		}

		#endregion

		#region 重写方法

		protected override string GetKeyForItem(T item)
		{
			return item.Name;
		}

		protected override void InsertItems(int index, IEnumerable<T> items)
		{
			foreach(var item in items)
			{
				item.InnerParent = _owner;
			}

			base.InsertItems(index, items);
		}

	    protected override void SetItem(int index, T item)
		{
			var oldItem = this.Items[index];

			if(oldItem != null)
				oldItem.InnerParent = null;

			item.InnerParent = _owner;

			base.SetItem(index, item);
		}

		protected override void RemoveItem(int index)
		{
			var item = this.Items[index];

			if(item != null)
				item.InnerParent = null;

			base.RemoveItem(index);
		}

		protected override void ClearItems()
		{
			foreach(var item in this.Items)
			{
				if(item != null)
					item.InnerParent = null;
			}

			base.ClearItems();
		}

		#endregion
	}
}
