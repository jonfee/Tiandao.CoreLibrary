using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
#if !CORE_CLR
	[Serializable]
#endif
	public class CommandExecutorExecutingEventArgs : CommandExecutorEventArgs
	{
		#region 私有字段

		private bool _cancel;

		#endregion

		#region 公共属性

		public bool Cancel
		{
			get
			{
				return _cancel;
			}
			set
			{
				_cancel = value;
			}
		}

		#endregion

		#region 构造方法

		public CommandExecutorExecutingEventArgs(CommandExecutorContextBase context, bool cancel = false) : base(context)
		{
			_cancel = cancel;
		}

		#endregion
	}
}
