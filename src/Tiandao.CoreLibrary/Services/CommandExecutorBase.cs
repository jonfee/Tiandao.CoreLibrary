using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
	public abstract class CommandExecutorBase<TContext> : ICommandExecutor where TContext : CommandExecutorContextBase //MarshalByRefObject, ICommandExecutor where TContext : CommandExecutorContextBase
	{
		#region 声明事件

		public event EventHandler<CommandExecutorExecutingEventArgs> Executing;
		public event EventHandler<CommandExecutorExecutedEventArgs> Executed;

		#endregion

		#region 私有字段

		private readonly CommandTreeNode _root;

		#endregion

		#region 公共属性

		public CommandTreeNode Root
		{
			get
			{
				return _root;
			}
		}

		#endregion

		#region 构造方法

		protected CommandExecutorBase()
		{
			_root = new CommandTreeNode();
		}

		#endregion

		#region 查找方法

		public virtual CommandTreeNode Find(string path)
		{
			return _root.Find(path);
		}

		#endregion

		#region 执行方法

		public object Execute(string commandText, object parameter = null)
		{
			if(string.IsNullOrWhiteSpace(commandText))
				throw new ArgumentNullException("commandText");

			//创建命令执行器上下文对象
			var context = this.CreateContext(commandText, parameter);

			if(context == null)
				throw new InvalidOperationException("The context of command-executor is null.");

			//调用执行请求
			return this.Execute(context);
		}

		protected virtual object Execute(TContext context)
		{
			if(context == null)
				throw new ArgumentNullException("context");

			//创建事件参数对象
			var executingArgs = new CommandExecutorExecutingEventArgs(context);

			//激发“Executing”事件
			this.OnExecuting(executingArgs);

			if(executingArgs.Cancel)
				return executingArgs.Result;

			//执行命令
			this.OnExecute(context);

			//创建事件参数对象
			var executedArgs = new CommandExecutorExecutedEventArgs(context);

			//激发“Executed”事件
			this.OnExecuted(executedArgs);

			//返回最终的执行结果
			return context.Result;
		}

		#endregion

		#region 执行实践

		protected abstract TContext CreateContext(string commandText, object parameter);

		protected virtual void OnExecute(TContext context)
		{
			var command = context.Command;

			if(command != null)
				context.Result = command.Execute(context.Parameter);
		}

		#endregion

		#region 激发事件

		protected virtual void OnExecuting(CommandExecutorExecutingEventArgs args)
		{
			var executing = this.Executing;

			if(executing != null)
				executing(this, args);
		}

		protected virtual void OnExecuted(CommandExecutorExecutedEventArgs args)
		{
			var executed = this.Executed;

			if(executed != null)
				executed(this, args);
		}

		#endregion
	}
}