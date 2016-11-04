using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public class ExecutionPipelineCollection : Collections.Collection<ExecutionPipeline>
	{
		#region 构造方法

		public ExecutionPipelineCollection()
		{
		}

		public ExecutionPipelineCollection(IEnumerable<ExecutionPipeline> pipelines) : base(pipelines)
		{
		}

		#endregion

		#region 重写方法

		protected override bool TryConvertItem(object value, out ExecutionPipeline item)
		{
			if(value is IExecutionHandler)
			{
				item = new ExecutionPipeline((IExecutionHandler)value);
				return true;
			}

			return base.TryConvertItem(value, out item);
		}

		#endregion

		#region 公共方法

		public ExecutionPipeline Add(IExecutionHandler handler, IPredication predication = null)
		{
			if(handler == null)
				throw new ArgumentNullException("handler");

			var item = new ExecutionPipeline(handler, predication);
			base.Add(item);

			return item;
		}

		#endregion
	}
}
