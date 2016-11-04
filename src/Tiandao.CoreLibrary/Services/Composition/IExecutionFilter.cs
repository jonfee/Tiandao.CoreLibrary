using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
	/// <summary>
	/// 提供过滤执行管道的功能。
	/// </summary>
	public interface IExecutionFilter
    {
		/// <summary>
		/// 获取执行过滤器的名称。
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// 表示在执行处理程序之前被激发调用。
		/// </summary>
		/// <param name="context">当前执行上下文对象。</param>
		void OnExecuting(IExecutionContext context);

		/// <summary>
		/// 表示在执行处理程序之后被激发调用。
		/// </summary>
		/// <param name="context">当前执行上下文对象。</param>
		void OnExecuted(IExecutionContext context);
	}
}
