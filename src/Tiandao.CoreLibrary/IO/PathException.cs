using System;
using System.Runtime.Serialization;

namespace Tiandao.IO
{
#if !CORE_CLR
	[Serializable]
	public class PathException : ApplicationException
#else
	public class PathException : Exception
#endif
	{
		#region 构造方法

		public PathException(string message) : base(message)
		{

		}

		public PathException(string message, Exception innerException) : base(message, innerException)
		{

		}

#if !CORE_CLR
		protected PathException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		
		}
#endif

		#endregion
	}
}
