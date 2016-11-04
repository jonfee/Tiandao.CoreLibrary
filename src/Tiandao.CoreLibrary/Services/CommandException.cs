using System;
using System.Runtime.Serialization;

namespace Tiandao.Services
{
#if !CORE_CLR
	[Serializable]
	public class CommandException : ApplicationException
#else
	public class CommandException : Exception
#endif
	{
		#region 私有字段

		private int _code;

		#endregion

		#region 公共属性

		public int Code
		{
			get
			{
				return _code;
			}
		}

		#endregion

		#region 构造方法

		public CommandException()
		{
			_code = 0;
		}

		public CommandException(string message) : this(0, message, null)
		{
		}

		public CommandException(string message, Exception innerException) : this(0, message, innerException)
		{
		}

		public CommandException(int code, string message) : this(code, message, null)
		{
		}

		public CommandException(int code, string message, Exception innerException) : base(message, innerException)
		{
			_code = code;
		}

#if !CORE_CLR
		protected CommandException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			_code = info.GetInt32("Code");
		}
#endif

		#endregion

		#region 重写方法

#if !CORE_CLR
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			//调用基类同名方法
			base.GetObjectData(info, context);

			//将定义的属性值加入持久化信息集中
			info.AddValue("Code", _code);
		}
#endif

		#endregion
	}
}
