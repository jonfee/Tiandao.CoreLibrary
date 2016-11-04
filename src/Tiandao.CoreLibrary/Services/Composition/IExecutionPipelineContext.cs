using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public interface IExecutionPipelineContext : IExecutionContext
	{
		/// <summary>
		/// 获取当前上下文所属的执行管道。
		/// </summary>
		ExecutionPipeline Pipeline
		{
			get;
		}

		/// <summary>
		/// 判断当前管道是否有子管道集。
		/// </summary>
		bool HasChildren
		{
			get;
		}

		/// <summary>
		/// 获取当前管道的子管道集合。
		/// </summary>
		ExecutionPipelineCollection Children
		{
			get;
		}
	}
}