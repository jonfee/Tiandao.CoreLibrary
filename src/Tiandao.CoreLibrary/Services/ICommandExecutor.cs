using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public interface ICommandExecutor
    {
		#region 事件定义

		event EventHandler<CommandExecutorExecutingEventArgs> Executing;
		event EventHandler<CommandExecutorExecutedEventArgs> Executed;

		#endregion

		#region 属性定义

		CommandTreeNode Root
		{
			get;
		}

		#endregion

		#region 方法定义

		object Execute(string commandPath, object parameter = null);

		CommandTreeNode Find(string commandPath);

		#endregion
	}
}
