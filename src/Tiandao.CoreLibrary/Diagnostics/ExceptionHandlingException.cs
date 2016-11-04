using System;
using System.Collections.Generic;

namespace Tiandao.Diagnostics
{
	/// <summary>
	/// 异常处理失败时引发的异常。
	/// </summary>
#if !CORE_CLR
	[Serializable]
	public class ExceptionHandlingException : ApplicationException
#else
	public class ExceptionHandlingException : Exception
#endif
	{
		#region 构造函数

		public ExceptionHandlingException() : this(string.Empty, null)
		{

		}

		public ExceptionHandlingException(string message) : this(message, null)
		{

		}

		public ExceptionHandlingException(string message, Exception innerException) : base(message, innerException)
		{

		}

		#endregion
	}
}
