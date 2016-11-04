using System;
using System.Collections.Generic;

namespace Tiandao.Diagnostics
{
    public class ExceptionHandledEventArgs : EventArgs
    {
		#region 公共属性

		public IExceptionHandler Handler
		{
			get;
			private set;
		}

		public Exception Exception
		{
			get;
			private set;
		}

		#endregion

		#region 构造方法

		public ExceptionHandledEventArgs(IExceptionHandler handler, Exception exception)
		{
			if(handler == null)
			{
				throw new ArgumentNullException("handler");
			}

			this.Handler = handler;
			this.Exception = exception;
		}

		#endregion
	}
}
