using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
#if !CORE_CLR
	public class ExecutionContext : MarshalByRefObject, IExecutionContext
#else
	public class ExecutionContext : IExecutionContext
#endif
	{
		#region 私有字段

		private IExecutor _executor;
		private object _parameter;
		private object _result;
		private Exception _exception;
		private IDictionary<string, object> _extendedProperties;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取处理本次执行请求的执行器。
		/// </summary>
		public virtual IExecutor Executor
		{
			get
			{
				return _executor;
			}
		}

		/// <summary>
		/// 获取处理本次执行请求的输入参数。
		/// </summary>
		public object Parameter
		{
			get
			{
				return _parameter;
			}
		}

		/// <summary>
		/// 获取本次执行中发生的异常。
		/// </summary>
		public virtual Exception Exception
		{
			get
			{
				return _exception;
			}
			internal protected set
			{
				_exception = value;
			}
		}

		/// <summary>
		/// 获取扩展属性集是否有内容。
		/// </summary>
		/// <remarks>
		///		<para>在不确定扩展属性集是否含有内容之前，建议先使用该属性来检测。</para>
		/// </remarks>
		public virtual bool HasExtendedProperties
		{
			get
			{
				return (_extendedProperties != null);
			}
		}

		/// <summary>
		/// 获取扩展属性集。
		/// </summary>
		public virtual IDictionary<string, object> ExtendedProperties
		{
			get
			{
				if(_extendedProperties == null)
					System.Threading.Interlocked.CompareExchange(ref _extendedProperties, new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase), null);

				return _extendedProperties;
			}
		}

		/// <summary>
		/// 获取或设置本次执行的返回结果。
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

		public ExecutionContext(IExecutor executor, object parameter = null, IDictionary<string, object> extendedProperties = null)
		{
			if(executor == null)
				throw new ArgumentNullException("executor");

			_executor = executor;
			_parameter = parameter;

			if(extendedProperties != null && extendedProperties.Count > 0)
				_extendedProperties = new Dictionary<string, object>(extendedProperties);
		}
		
		#endregion
	}
}