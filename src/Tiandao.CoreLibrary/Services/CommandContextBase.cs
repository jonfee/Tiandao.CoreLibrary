using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
#if !CORE_CLR
	public class CommandContextBase : MarshalByRefObject
#else
	public class CommandContextBase
#endif
	{
		#region 私有字段

		private ICommand _command;
		private CommandTreeNode _commandNode;
		private object _parameter;
		private IDictionary<string, object> _extendedProperties;
		private ICommandExecutor _executor;
		private IDictionary<ICommandExecutor, Dictionary<string, object>> _statesProvider;
		private object _result;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取执行的命令对象。
		/// </summary>
		public ICommand Command
		{
			get
			{
				return _command;
			}
		}

		/// <summary>
		/// 获取执行的命令所在节点。
		/// </summary>
		public CommandTreeNode CommandNode
		{
			get
			{
				return _commandNode;
			}
		}

		/// <summary>
		/// 获取命令执行的传入参数。
		/// </summary>
		public object Parameter
		{
			get
			{
				return _parameter;
			}
		}

		/// <summary>
		/// 获取扩展属性集是否有内容。
		/// </summary>
		/// <remarks>
		///		<para>在不确定扩展属性集是否含有内容之前，建议先使用该属性来检测。</para>
		/// </remarks>
		public bool HasExtendedProperties
		{
			get
			{
				return _extendedProperties != null && _extendedProperties.Count > 0;
			}
		}

		/// <summary>
		/// 获取可用于在本次执行过程中在各处理模块之间组织和共享数据的键/值集合。
		/// </summary>
		public IDictionary<string, object> ExtendedProperties
		{
			get
			{
				if(_extendedProperties == null)
					System.Threading.Interlocked.CompareExchange(ref _extendedProperties, new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase), null);

				return _extendedProperties;
			}
		}

		/// <summary>
		/// 获取执行命令所在的命令执行器。
		/// </summary>
		public ICommandExecutor Executor
		{
			get
			{
				return _executor;
			}
		}

		/// <summary>
		/// 获取一个由当前命令执行器为宿主的字典容器。
		/// </summary>
		/// <remarks>
		///		<para>在本属性返回的字典集合中的内容对于相同<see cref="ICommandExecutor"/>中的命令而言都是可见(读写)的，但对于不同<seealso cref="ICommandExecutor"/>下的命令而言，这些字典集合内的内容则是不可见的。</para>
		/// </remarks>
		public IDictionary<string, object> States
		{
			get
			{
				if(_statesProvider == null)
					System.Threading.Interlocked.CompareExchange(ref _statesProvider, new Dictionary<ICommandExecutor, Dictionary<string, object>>(), null);

				Dictionary<string, object> states;
				if(_statesProvider.TryGetValue(_executor, out states))
					return states;

				lock (_statesProvider)
				{
					if(_statesProvider.TryGetValue(_executor, out states))
						return states;

					states = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
					_statesProvider.Add(_executor, states);
				}

				return states;
			}
		}

		/// <summary>
		/// 获取或设置命令的执行结果。
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

		protected CommandContextBase(ICommandExecutor executor, ICommand command, object parameter, IDictionary<string, object> extendedProperties = null)
		{
			if(command == null)
				throw new ArgumentNullException("command");

			_command = command;
			_parameter = parameter;
			_extendedProperties = extendedProperties;
			_executor = executor;
		}

		protected CommandContextBase(ICommandExecutor executor, CommandTreeNode commandNode, object parameter, IDictionary<string, object> extendedProperties = null)
		{
			if(commandNode == null)
				throw new ArgumentNullException("commandNode");

			if(commandNode.Command == null)
				throw new ArgumentException(string.Format("The Command property of '{0}' command-node is null.", commandNode.FullPath));

			_commandNode = commandNode;
			_command = commandNode.Command;
			_parameter = parameter;
			_extendedProperties = extendedProperties;
			_executor = executor;
		}
		
		#endregion
	}
}
