using System;

namespace Tiandao.Serialization
{
    public class SerializationWroteEventArgs : EventArgs
	{
		#region 私有字段

		private SerializationWriterContext _context;

		#endregion

		#region 公共属性

		public SerializationWriterContext Context
		{
			get
			{
				return _context;
			}
		}

		#endregion

		#region 构造方法

		public SerializationWroteEventArgs(SerializationWriterContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			_context = context;
		}

		#endregion
	}
}