using System;

namespace Tiandao.Collections
{
	#if !CORE_CLR
	[Serializable]
	#endif
	public class DequeuedEventArgs : EventArgs
	{
		#region 私有字段

		private object _value;
		private bool _isMultiple;
		private CollectionRemovedReason _reason;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取出队的内容值。
		/// </summary>
		public object Value
		{
			get
			{
				return _value;
			}
		}

		/// <summary>
		/// 获取一个指示本次出队是否为批量出队操作，如果为批量出队则<see cref="Value"/>属性返回的则是多值。
		/// </summary>
		public bool IsMultiple
		{
			get
			{
				return _isMultiple;
			}
		}

		/// <summary>
		/// 获取出队事件被激发的原因。
		/// </summary>
		public CollectionRemovedReason Reason
		{
			get
			{
				return _reason;
			}
		}
		
		#endregion

		#region 构造方法

		public DequeuedEventArgs(object value, bool isMultiple, CollectionRemovedReason reason)
		{
			_value = value;
			_isMultiple = isMultiple;
			_reason = reason;
		}
		
		#endregion
	}
}
