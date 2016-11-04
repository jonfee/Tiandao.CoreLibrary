using System;
using System.Collections.Generic;

namespace Tiandao.Services.Composition
{
    public class ExecutionFilterCollection : Collections.NamedCollectionBase<IExecutionFilter>
	{
		#region 构造方法

		public ExecutionFilterCollection()
		{
		}

		#endregion

		#region 重写方法

		protected override string GetKeyForItem(IExecutionFilter item)
		{
			return item.Name;
		}

		#endregion
	}
}