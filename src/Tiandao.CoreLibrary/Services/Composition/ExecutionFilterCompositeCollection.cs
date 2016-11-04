using System;
using System.Collections.Generic;
using System.Linq;

namespace Tiandao.Services.Composition
{
    public class ExecutionFilterCompositeCollection : Collections.NamedCollectionBase<ExecutionFilterComposite>, ICollection<IExecutionFilter>
	{
		#region 构造方法

		public ExecutionFilterCompositeCollection() : base(StringComparer.OrdinalIgnoreCase)
		{
		}

		#endregion

		#region 公共方法

		public ExecutionFilterComposite Add(IExecutionFilter filter)
		{
			return this.Add(filter, null);
		}

		public ExecutionFilterComposite Add(IExecutionFilter filter, IPredication predication)
		{
			if(filter == null)
				throw new ArgumentNullException("filter");

			var result = new ExecutionFilterComposite(filter, predication);
			base.Add(result);

			return result;
		}

		#endregion

		#region 重写方法

		protected override string GetKeyForItem(ExecutionFilterComposite item)
		{
			return item.Filter.Name;
		}

		protected override bool TryConvertItem(object value, out ExecutionFilterComposite item)
		{
			if(value is IExecutionFilter)
			{
				item = new ExecutionFilterComposite((IExecutionFilter)value);
				return true;
			}

			return base.TryConvertItem(value, out item);
		}

		#endregion

		#region 接口实现

		void ICollection<IExecutionFilter>.Add(IExecutionFilter item)
		{
			if(item == null)
				throw new ArgumentNullException("item");

			base.Add(new ExecutionFilterComposite(item, null));
		}

		public bool Contains(IExecutionFilter item)
		{
			if(item == null)
				return false;

			return base.Items.Any(p => p.Filter == item);
		}

		public void CopyTo(IExecutionFilter[] array, int arrayIndex)
		{
			if(array == null)
				throw new ArgumentNullException("array");

			if(arrayIndex < 0 || arrayIndex >= array.Length)
				throw new ArgumentOutOfRangeException("arrayIndex");

			for(int i = arrayIndex; i < array.Length; i++)
			{
				array[i] = base.Items[i - arrayIndex];
			}
		}

		public bool Remove(IExecutionFilter item)
		{
			if(item == null)
				return false;

			for(int i = 0; i < base.Items.Count; i++)
			{
				if(base.Items[i].Filter == item)
				{
					base.Items.RemoveAt(i);
					return true;
				}
			}

			return false;
		}

		IEnumerator<IExecutionFilter> IEnumerable<IExecutionFilter>.GetEnumerator()
		{
			foreach(var item in base.Items)
				yield return item.Filter;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return base.GetEnumerator();
		}

		#endregion
	}
}
