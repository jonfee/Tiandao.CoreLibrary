using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public class ExecutionPipelineExecutingEventArgs : ExecutionEventArgs<IExecutionPipelineContext>
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

		public ExecutionPipelineExecutingEventArgs(IExecutionPipelineContext context, bool cancel = false) : base(context)
		{
			_cancel = cancel;
		}

		#endregion
	}
}
