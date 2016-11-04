using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
#if !CORE_CLR
	public class CommandExecutorContextBase : MarshalByRefObject
#else
	public class CommandExecutorContextBase
#endif
	{
		#region 私有字段

		private ICommandExecutor _executor;
		private string _commandText;
		private CommandTreeNode _commandNode;
		private ICommand _command;
		private object _parameter;
		private object _result;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取当前命令执行器对象。
		/// </summary>
		public ICommandExecutor Executor
		{
			get
			{
				return _executor;
			}
		}

		/// <summary>
		/// 获取当前执行的命令文本。
		/// </summary>
		public virtual string CommandText
		{
			get
			{
				return _commandText;
			}
		}

		/// <summary>
		/// 获取当前执行命令所在的命令树节点。
		/// </summary>
		public CommandTreeNode CommandNode
		{
			get
			{
				return _commandNode;
			}
		}

		/// <summary>
		/// 获取当前执行命令。
		/// </summary>
		public ICommand Command
		{
			get
			{
				return _command;
			}
		}

		/// <summary>
		/// 获取从命令执行器传入的参数值。
		/// </summary>
		public object Parameter
		{
			get
			{
				return _parameter;
			}
		}

		/// <summary>
		/// 获取或设置命令执行器的执行结果。
		/// </summary>
		public object Result
		{
			get
			{
				return _result;
			}
			set
			{
				_result = value;
			}
		}

		#endregion

		#region 构造方法

		protected CommandExecutorContextBase(ICommandExecutor executor, string commandText, CommandTreeNode commandNode, object parameter)
		{
			if(executor == null)
				throw new ArgumentNullException(nameof(executor));

			_executor = executor;
			_commandText = commandText;
			_commandNode = commandNode;
			_command = commandNode == null ? null : commandNode.Command;
			_parameter = parameter;
		}

		#endregion
	}
}