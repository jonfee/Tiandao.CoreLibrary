using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tiandao.Options.Profiles
{
    public class ProfileException : Exception
	{
		#region 构造方法

		public ProfileException()
		{
		}

		public ProfileException(string message) : base(message)
		{
		}

		public ProfileException(string message, Exception innerException) : base(message, innerException)
		{

		}

#if !CORE_CLR
		protected ProfileException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
#endif
		#endregion
	}
}
