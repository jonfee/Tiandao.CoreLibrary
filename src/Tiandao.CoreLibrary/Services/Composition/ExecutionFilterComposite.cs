using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public class ExecutionFilterComposite : IExecutionFilter
	{
		#region 私有字段

		private IExecutionFilter _filter;
		private IPredication _predication;

		#endregion

		#region 公共属性

		public virtual string Name
		{
			get
			{
				return _filter.Name;
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

		public IExecutionFilter Filter
		{
			get
			{
				return _filter;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException();

				_filter = value;
			}
		}

		#endregion

		#region 构造方法

		public ExecutionFilterComposite(IExecutionFilter filter) : this(filter, null)
		{
		}

		public ExecutionFilterComposite(IExecutionFilter filter, IPredication predication)
		{
			if(filter == null)
				throw new ArgumentNullException("filter");

			_filter = filter;
			_predication = predication;
		}

		#endregion

		#region 显式实现

		void IExecutionFilter.OnExecuting(IExecutionContext context)
		{
			var predication = this.Predication;

			if(predication == null || predication.Predicate(context))
				_filter.OnExecuting(context);
		}

		void IExecutionFilter.OnExecuted(IExecutionContext context)
		{
			var predication = this.Predication;

			if(predication == null || predication.Predicate(context))
				_filter.OnExecuted(context);
		}

		#endregion
	}
}
