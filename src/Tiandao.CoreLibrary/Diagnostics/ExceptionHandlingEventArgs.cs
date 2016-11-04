using System;
using System.Collections.Generic;

namespace Tiandao.Diagnostics
{
    public class ExceptionHandlingEventArgs : ExceptionHandledEventArgs
	{
		#region 公共属性

		public bool Cancel
		{
			get;
			set;
		}

		#endregion

		#region 构造方法

		public ExceptionHandlingEventArgs(IExceptionHandler handler, Exception exception) : this(handler, exception, false)
		{
		}

		public ExceptionHandlingEventArgs(IExceptionHandler handler, Exception exception, bool cancel) : base(handler, exception)
		{
			this.Cancel = cancel;
		}

		#endregion
	}
}
