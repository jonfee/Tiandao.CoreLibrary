using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public class ExecutedEventArgs : ExecutionEventArgs<IExecutionContext>
	{
		#region 构造函数

		public ExecutedEventArgs(IExecutionContext context) : base(context)
		{
		}

		#endregion
	}
}