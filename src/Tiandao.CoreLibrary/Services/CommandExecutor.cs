using System;

namespace Tiandao.Services
{
    public class CommandExecutor : CommandExecutorBase<CommandExecutorContext>
	{
		#region 静态字段

		private static CommandExecutor _default;

		#endregion

		#region 静态属性

		/// <summary>
		/// 获取或设置默认的<see cref="CommandExecutor"/>命令执行器。
		/// </summary>
		/// <remarks>
		///		<para>注意：如果已经设置过该属性，则不允许再更改其值。</para>
		/// </remarks>
		public static CommandExecutor Default
		{
			get
			{
				return _default;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException();

				if(_default == null)
					System.Threading.Interlocked.CompareExchange(ref _default, value, null);
			}
		}

		#endregion

		#region 私有字段

		private ICommandLineParser _parser;

		#endregion

		#region 公共属性

		public ICommandLineParser Parser
		{
			get
			{
				return _parser ?? CommandLine.CommandLineParser.Instance;
			}
			set
			{
				_parser = value;
			}
		}

		#endregion

		#region 构造方法

		public CommandExecutor()
		{

		}

		#endregion

		#region 重写方法

		protected override void OnExecute(CommandExecutorContext context)
		{
			var command = context.Command;

			if(command != null)
				context.Result = command.Execute(new CommandContext(this, context.CommandLine, context.CommandNode, context.Parameter));
		}

		protected override CommandExecutorContext CreateContext(string commandText, object parameter)
		{
			//解析当前命令文本
			var commandLine = this.OnParse(commandText);

			if(commandLine == null)
				throw new ArgumentException(string.Format("Invalid command-line text format of <{0}>.", commandText));

			//查找指定路径的命令对象
			var commandNode = this.Find(commandLine.FullPath);

			//如果指定的路径在命令树中是不存在的则抛出异常
			if(commandNode == null)
				throw new CommandNotFoundException(commandText);

			return new CommandExecutorContext(this, commandLine, commandNode, parameter);
		}

		#endregion

		#region 虚拟方法

		protected virtual CommandLine OnParse(string commandText)
		{
			var parser = this.Parser;

			return parser == null ? null : parser.Parse(commandText);
		}

		#endregion
	}
}
