using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public class ExecutionEventArgs<TContext> : EventArgs where TContext : IExecutionContext
	{
		#region 私有字段

		private TContext _context;

		#endregion

		#region 公共属性

		public TContext Context
		{
			get
			{
				return _context;
			}
		}

		#endregion

		#region 构造方法

		public ExecutionEventArgs(TContext context)
		{
			if(context == null)
				throw new ArgumentNullException("context");

			_context = context;
		}

		#endregion
	}
}
