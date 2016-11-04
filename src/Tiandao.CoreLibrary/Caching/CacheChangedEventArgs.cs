using System;

namespace Tiandao.Caching
{
#if !CORE_CLR
	[Serializable]
#endif
	public class CacheChangedEventArgs : EventArgs
    {
		#region 私有字段

		private CacheChangedReason _reason;
		private string _oldKey;
		private string _newKey;
		private object _oldValue;
		private object _newValue;

		#endregion

		#region 公共属性

		public CacheChangedReason Reason
		{
			get
			{
				return _reason;
			}
		}

		public string OldKey
		{
			get
			{
				return _oldKey;
			}
		}

		public object OldValue
		{
			get
			{
				return _oldValue;
			}
		}

		public string NewKey
		{
			get
			{
				return _newKey;
			}
		}

		public object NewValue
		{
			get
			{
				return _newValue;
			}
		}

		#endregion

		#region 构造方法

		public CacheChangedEventArgs(CacheChangedReason reason, string oldKey, object oldValue, string newKey = null, object newValue = null)
		{
			_reason = reason;
			_oldKey = oldKey;
			_oldValue = oldValue;
			_newKey = newKey;
			_newValue = newValue;
		}
		
		#endregion
	}
}
