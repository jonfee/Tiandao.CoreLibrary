using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public interface IExecutor
    {
		#region 事件定义

		/// <summary>表示执行器对所有管道执行完成之后激发的事件。</summary>
		event EventHandler<ExecutedEventArgs> Executed;
		/// <summary>表示执行器在执行操作之前激发的事件，该事件位于所有管道及过滤器之前被激发。</summary>
		event EventHandler<ExecutingEventArgs> Executing;

		/// <summary>表示管道执行完成，为管道执行步骤中最后一个被激发的事件。</summary>
		event EventHandler<ExecutionPipelineExecutedEventArgs> PipelineExecuted;
		/// <summary>表示管道开始执行，为管道执行步骤中第一个被激发的事件。</summary>
		event EventHandler<ExecutionPipelineExecutingEventArgs> PipelineExecuting;

		/// <summary>表示管道中的处理器执行完成。</summary>
		event EventHandler<ExecutionPipelineExecutedEventArgs> HandlerExecuted;
		/// <summary>表示管道中的处理器开始执行。</summary>
		event EventHandler<ExecutionPipelineExecutingEventArgs> HandlerExecuting;

		#endregion

		#region 属性定义

		/// <summary>
		/// 获取或设置一个<see cref="IExecutionCombiner"/>结果合并器。
		/// </summary>
		IExecutionCombiner Combiner
		{
			get;
			set;
		}

		/// <summary>
		/// 获取或设置一个<see cref="IExecutionPipelineSelector"/>管道执行选择器。
		/// </summary>
		IExecutionPipelineSelector PipelineSelector
		{
			get;
			set;
		}

		/// <summary>
		/// 获取当前执行器的管道集合。
		/// </summary>
		ExecutionPipelineCollection Pipelines
		{
			get;
		}

		/// <summary>
		/// 获取当前执行器的全局过滤器集合。
		/// </summary>
		/// <remarks>
		///		<para>全局过滤器即表示，当执行器被执行时全局过滤器优先于管道自身的过滤器被执行。</para>
		/// </remarks>
		ExecutionFilterCompositeCollection Filters
		{
			get;
		}

		#endregion

		#region 方法定义

		/// <summary>
		/// 执行方法。
		/// </summary>
		/// <param name="parameter">指定的执行参数对象。</param>
		/// <returns>返回执行的结果器。</returns>
		ExecutionResult Execute(object parameter = null);

		#endregion
	}
}
