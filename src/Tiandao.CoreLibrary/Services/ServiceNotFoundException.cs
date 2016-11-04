using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
#if !CORE_CLR
	public class ServiceNotFoundException : ApplicationException
#else
	public class ServiceNotFoundException : Exception 
#endif
	{
		#region 构造方法

		public ServiceNotFoundException()
		{

		}

		public ServiceNotFoundException(string message) : base(message)
		{

		}

		public ServiceNotFoundException(string message, Exception innerException) : base(message, innerException)
		{

		}

		#endregion
	}
}
