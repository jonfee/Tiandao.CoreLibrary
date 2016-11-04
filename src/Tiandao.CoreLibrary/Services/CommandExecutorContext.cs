using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public class CommandExecutorContext : CommandExecutorContextBase
	{
		#region 私有字段

		private CommandLine _commandLine;

		#endregion

		#region 公共属性

		public override string CommandText
		{
			get
			{
				return _commandLine.ToString();
			}
		}

		public CommandLine CommandLine
		{
			get
			{
				return _commandLine;
			}
		}

		#endregion

		#region 构造方法

		public CommandExecutorContext(ICommandExecutor executor, CommandLine commandLine, CommandTreeNode commandNode, object parameter) : base(executor, null, commandNode, parameter)
		{
			if(commandLine == null)
				throw new ArgumentNullException(nameof(commandLine));

			_commandLine = commandLine;
		}

		#endregion
	}
}