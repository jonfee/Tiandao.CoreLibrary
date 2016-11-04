using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
#if !CORE_CLR
	[Serializable]
#endif
	public class CommandExecutorExecutedEventArgs : CommandExecutorEventArgs
	{
		#region 构造方法

		public CommandExecutorExecutedEventArgs(CommandExecutorContextBase context) : base(context)
		{

		}

		#endregion
	}
}
