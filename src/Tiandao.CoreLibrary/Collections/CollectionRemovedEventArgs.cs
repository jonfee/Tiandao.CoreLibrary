using System;
using System.Collections.Generic;

namespace Tiandao.Collections
{
    public class CollectionRemovedEventArgs : EventArgs
	{
		#region 私有字段

		private CollectionRemovedReason _reason;
		private object _item;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取被删除的原因。
		/// </summary>
		public CollectionRemovedReason Reason
		{
			get
			{
				return _reason;
			}
		}

		/// <summary>
		/// 获取被删除的集合元素。
		/// </summary>
		public object Item
		{
			get
			{
				return _item;
			}
		}

		#endregion

		#region 构造方法

		public CollectionRemovedEventArgs(CollectionRemovedReason reason, object item)
		{
			_reason = reason;
			_item = item;
		}

		#endregion
	}
}