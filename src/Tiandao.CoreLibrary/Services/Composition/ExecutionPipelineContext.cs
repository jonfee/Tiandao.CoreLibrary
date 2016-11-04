using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public class ExecutionPipelineContext : ExecutionContext, IExecutionPipelineContext
	{
		#region 私有字段

		private IExecutionContext _context;
		private ExecutionPipeline _pipeline;
		private ExecutionPipelineCollection _children;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取当前管道上下文的上级执行上下文。
		/// </summary>
		public IExecutionContext Context
		{
			get
			{
				return _context;
			}
		}

		/// <summary>
		/// 获取当前的<see cref="ExecutionPipeline"/>执行管道。
		/// </summary>
		public ExecutionPipeline Pipeline
		{
			get
			{
				return _pipeline;
			}
		}

		public virtual bool HasChildren
		{
			get
			{
				if(_children == null)
					return _pipeline.HasChildren;

				return _children != null && _children.Count > 0;
			}
		}

		public virtual ExecutionPipelineCollection Children
		{
			get
			{
				if(_children == null)
				{
					if(_pipeline.HasChildren)
						System.Threading.Interlocked.CompareExchange(ref _children, new ExecutionPipelineCollection(_pipeline.Children), null);
					else
						System.Threading.Interlocked.CompareExchange(ref _children, new ExecutionPipelineCollection(), null);
				}

				return _children;
			}
		}

		#endregion

		#region 构造方法

		public ExecutionPipelineContext(IExecutionContext context, ExecutionPipeline pipeline, object parameter) : base(context.Executor, parameter)
		{
			if(pipeline == null)
				throw new ArgumentNullException("pipeline");

			_context = context;
			_pipeline = pipeline;
		}

		#endregion
	}
}
