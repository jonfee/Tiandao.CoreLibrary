using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public class CommandContext : CommandContextBase
    {
		#region 私有字段

		private CommandLine _commandLine;
		private CommandOptionCollection _options;

		#endregion

		#region 公共属性

		public CommandLine CommandLine
		{
			get
			{
				return _commandLine;
			}
		}

		public CommandOptionCollection Options
		{
			get
			{
				return _options;
			}
		}

		public string[] Arguments
		{
			get
			{
				if(_commandLine == null)
					return new string[0];
				else
					return _commandLine.Arguments;
			}
		}

		#endregion

		#region 构造方法

		public CommandContext(ICommandExecutor executor, CommandLine commandLine, ICommand command, object parameter, IDictionary<string, object> items = null) : base(executor, command, parameter, items)
		{
			_commandLine = commandLine;

			if(commandLine == null)
				_options = new CommandOptionCollection(command);
			else
				_options = new CommandOptionCollection(command, (System.Collections.IDictionary)commandLine.Options);
		}

		public CommandContext(ICommandExecutor executor, CommandLine commandLine, CommandTreeNode commandNode, object parameter, IDictionary<string, object> items = null) : base(executor, commandNode, parameter, items)
		{
			_commandLine = commandLine;

			if(commandLine == null)
				_options = new CommandOptionCollection(this.Command);
			else
				_options = new CommandOptionCollection(this.Command, (System.Collections.IDictionary)commandLine.Options);
		}
		
		#endregion
	}
}