using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
#if !CORE_CLR
	[Serializable]
#endif
	public class CommandExecutorEventArgs : EventArgs
	{
		#region 私有字段

		private CommandExecutorContextBase _context;

		#endregion

		#region 公共属性

		public CommandExecutorContextBase Context
		{
			get
			{
				return _context;
			}
		}

		public ICommandExecutor CommandExecutor
		{
			get
			{
				return _context.Executor;
			}
		}

		public string CommandText
		{
			get
			{
				return _context.CommandText;
			}
		}

		public object Parameter
		{
			get
			{
				return _context.Parameter;
			}
		}

		public CommandTreeNode CommandNode
		{
			get
			{
				return _context.CommandNode;
			}
		}

		public ICommand Command
		{
			get
			{
				return _context.Command;
			}
		}

		public object Result
		{
			get
			{
				return _context.Result;
			}
			set
			{
				_context.Result = value;
			}
		}

		#endregion

		#region 构造方法

		public CommandExecutorEventArgs(CommandExecutorContextBase context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			_context = context;
		}

		#endregion
	}
}
