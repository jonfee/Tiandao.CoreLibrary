using System;
using System.Collections.Generic;
using System.Linq;

namespace Tiandao.Options.Profiles
{
    public class ProfileCommentCollection : ICollection<ProfileComment>
	{
		#region 私有字段

		private ProfileItemCollection _items;

		#endregion

		#region 公共属性

		public int Count
		{
			get
			{
				return _items.Count(item => item.ItemType == ProfileItemType.Comment);
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return _items.IsReadOnly;
			}
		}

		#endregion

		#region 构造方法

		public ProfileCommentCollection(ProfileItemCollection items)
		{
			if(items == null)
				throw new ArgumentNullException("items");

			_items = items;
		}

		#endregion

		#region 公共方法

		public ProfileComment Add(string comment, int lineNumber = -1)
		{
			if(comment == null)
				return null;

			var item = new ProfileComment(comment, lineNumber);
			_items.Add(item);
			return item;
		}

		public void Add(ProfileComment item)
		{
			_items.Add(item);
		}

		public void Clear()
		{
			foreach(var item in _items)
			{
				if(item.ItemType == ProfileItemType.Comment)
					_items.Remove(item);
			}
		}

		public bool Contains(ProfileComment item)
		{
			return _items.Contains(item);
		}

		public void CopyTo(ProfileComment[] array, int arrayIndex)
		{
			if(array == null)
				return;

			if(arrayIndex >= array.Length)
				throw new ArgumentOutOfRangeException("arrayIndex");

			int index = 0;

			foreach(var item in _items)
			{
				if(arrayIndex + index >= array.Length)
					return;

				if(item.ItemType == ProfileItemType.Comment)
					array[arrayIndex + index++] = (ProfileComment)item;
			}
		}

		public bool Remove(ProfileComment item)
		{
			return _items.Remove(item);
		}

		#endregion

		#region 遍历枚举

		public IEnumerator<ProfileComment> GetEnumerator()
		{
			foreach(var item in _items)
			{
				if(item.ItemType == ProfileItemType.Comment)
					yield return (ProfileComment)item;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
