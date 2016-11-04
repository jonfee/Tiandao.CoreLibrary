using System;
using System.Collections.Generic;

namespace Tiandao.Diagnostics
{
#if !CORE_CLR
	[Serializable]
#endif
	public class FailureEventArgs : EventArgs
	{
		#region 私有字段

		private Exception _exception;
		private string _message;
		private object _state;

		#endregion

		#region 公共属性

		public Exception Exception
		{
			get
			{
				return _exception;
			}
		}

		public string Message
		{
			get
			{
				if(string.IsNullOrEmpty(_message))
				{
					return _exception == null ? string.Empty : _exception.Message;
				}

				return _message ?? string.Empty;
			}
		}

		public object State
		{
			get
			{
				return _state;
			}
		}

		#endregion

		#region 构造方法

		public FailureEventArgs(string message) : this(message, null)
		{
		}

		public FailureEventArgs(string message, object state)
		{
			_exception = null;
			_message = message;
			_state = state;
		}

		public FailureEventArgs(Exception exception) : this(exception, null)
		{
		}

		public FailureEventArgs(Exception exception, object state)
		{
			_exception = exception;
			_state = state;
		}

		#endregion
	}
}
