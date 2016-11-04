using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
#if !CORE_CLR
	public class ExecutionPipeline : MarshalByRefObject
#else
	public class ExecutionPipeline
#endif
	{
		#region 私有字段

		private IPredication _predication;
		private IExecutionHandler _handler;
		private ExecutionPipelineCollection _children;
		private ExecutionFilterCompositeCollection _filters;

		#endregion

		#region 公共属性

		public bool HasChildren
		{
			get
			{
				return _children != null && _children.Count > 0;
			}
		}

		/// <summary>
		/// 获取当前管道的后续管道集合。
		/// </summary>
		public ExecutionPipelineCollection Children
		{
			get
			{
				if(_children == null)
					System.Threading.Interlocked.CompareExchange(ref _children, new ExecutionPipelineCollection(), null);

				return _children;
			}
		}

		public bool HasFilters
		{
			get
			{
				return _filters != null && _filters.Count > 0;
			}
		}

		/// <summary>
		/// 获取当前管道的过滤器集合。
		/// </summary>
		public ExecutionFilterCompositeCollection Filters
		{
			get
			{
				if(_filters == null)
					System.Threading.Interlocked.CompareExchange(ref _filters, new ExecutionFilterCompositeCollection(), null);

				return _filters;
			}
		}

		public IPredication Predication
		{
			get
			{
				return _predication;
			}
			set
			{
				_predication = value;
			}
		}

		public IExecutionHandler Handler
		{
			get
			{
				return _handler;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException();

				_handler = value;
			}
		}

		#endregion

		#region 构造方法

		public ExecutionPipeline() : this(null, null)
		{
		}

		public ExecutionPipeline(IExecutionHandler handler, IPredication predication = null)
		{
			_handler = handler;
			_predication = predication;
		}

		#endregion
	}
}
