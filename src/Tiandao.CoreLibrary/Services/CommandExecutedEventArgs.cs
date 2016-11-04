using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
#if !CORE_CLR
	[Serializable]
#endif
	public class CommandExecutedEventArgs : EventArgs
    {
		#region 私有字段

		private CommandContextBase _context;
		private object _parameter;
		private object _result;
		private IDictionary<string, object> _extendedProperties;
		private bool _exceptionHandled;
		private Exception _exception;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取命令执行过程中的异常，如果返回空则表示为发生异常。
		/// </summary>
		public Exception Exception
		{
			get
			{
				return _exception;
			}
		}

		/// <summary>
		/// 获取或设置异常是否处理完成，如果返回假(false)则异常信息将被抛出。
		/// </summary>
		public bool ExceptionHandled
		{
			get
			{
				return _exceptionHandled;
			}
			set
			{
				_exceptionHandled = value;
			}
		}

		/// <summary>
		/// 获取命令的执行上下文对象。
		/// </summary>
		public CommandContextBase Context
		{
			get
			{
				return _context;
			}
		}

		/// <summary>
		/// 获取命令的执行参数对象。
		/// </summary>
		public object Parameter
		{
			get
			{
				return _parameter;
			}
		}

		public object Result
		{
			get
			{
				return _result;
			}
			set
			{
				_result = value;

				if(_context != null)
					_context.Result = value;
			}
		}

		public bool HasExtendedProperties
		{
			get
			{
				return _extendedProperties != null && _extendedProperties.Count > 0;
			}
		}

		/// <summary>
		/// 获取可用于在命令执行过程中在各处理模块之间组织和共享数据的键/值集合。
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

		#endregion

		#region 构造方法

		public CommandExecutedEventArgs(CommandContextBase context, Exception exception = null)
		{
			if(context != null)
			{
				_context = context;
				_parameter = context.Parameter;
				_result = context.Result;
				_extendedProperties = context.HasExtendedProperties ? context.ExtendedProperties : null;
			}

			_exception = exception;
		}

		/// <summary>
		/// 构造一个命令执行成功的事件参数对象。
		/// </summary>
		/// <param name="parameter">命令执行参数对象。</param>
		/// <param name="result">命令执行的结果。</param>
		/// <param name="extendedProperties">指定的扩展属性集。</param>
		public CommandExecutedEventArgs(object parameter, object result, IDictionary<string, object> extendedProperties = null)
		{
			var context = parameter as CommandContextBase;

			if(context != null)
			{
				_context = context;
				_parameter = context.Parameter;
				_result = result ?? context.Result;
				_extendedProperties = extendedProperties ?? (context.HasExtendedProperties ? context.ExtendedProperties : null);
			}
			else
			{
				_parameter = parameter;
				_result = result;
				_extendedProperties = extendedProperties;
			}
		}

		/// <summary>
		/// 构造一个命令执行失败的事件参数对象。
		/// </summary>
		/// <param name="parameter">命令执行参数对象。</param>
		/// <param name="exception">命令执行失败的异常对象。</param>
		/// <param name="extendedProperties">指定的扩展属性集。</param>
		public CommandExecutedEventArgs(object parameter, Exception exception, IDictionary<string, object> extendedProperties = null)
		{
			var context = parameter as CommandContextBase;

			if(context != null)
			{
				_context = context;
				_parameter = context.Parameter;
				_extendedProperties = extendedProperties ?? (context.HasExtendedProperties ? context.ExtendedProperties : null);
			}
			else
			{
				_parameter = parameter;
				_extendedProperties = extendedProperties;
			}

			_exception = exception;
		}

		#endregion
	}
}
