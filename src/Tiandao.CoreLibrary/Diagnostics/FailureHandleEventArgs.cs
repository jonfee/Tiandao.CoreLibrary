using System;
using System.Collections.Generic;

namespace Tiandao.Diagnostics
{
#if !CORE_CLR
	[Serializable]
#endif
	public class FailureHandleEventArgs : FailureEventArgs
	{
		#region 私有字段

		private bool _handled;

		#endregion

		#region 公共属性

		public bool Handled
		{
			get
			{
				return _handled;
			}
			set
			{
				_handled = value;
			}
		}

		#endregion

		#region 构造方法

		public FailureHandleEventArgs(Exception exception) : this(exception, null, false)
		{
		}

		public FailureHandleEventArgs(Exception exception, object state) : this(exception, state, false)
		{
		}

		public FailureHandleEventArgs(Exception exception, object state, bool handled) : base(exception, state)
		{
			_handled = handled;
		}

		public FailureHandleEventArgs(string message) : this(message, null, false)
		{
		}

		public FailureHandleEventArgs(string message, object state) : this(message, state, false)
		{
		}

		public FailureHandleEventArgs(string message, object state, bool handled) : base(message, state)
		{
			_handled = handled;
		}

		#endregion
	}
}
