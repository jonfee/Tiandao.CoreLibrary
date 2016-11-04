using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public class PredicationCollection<T> : Collections.Collection<IPredication<T>>, IPredication<T>
	{
		#region 私有字段

		private PredicationCombination _combine;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取或设置断言集合内各断言的逻辑组合方式。
		/// </summary>
		public PredicationCombination Combination
		{
			get
			{
				return _combine;
			}
			set
			{
				_combine = value;
			}
		}

		#endregion

		#region 构造方法

		public PredicationCollection() : this(PredicationCombination.Or)
		{

		}

		public PredicationCollection(PredicationCombination combine)
		{
			_combine = combine;
		}

		#endregion

		#region 参数转换

		protected virtual bool TryConertParameter(object parameter, out T result)
		{
			return Common.Converter.TryConvertValue<T>(parameter, out result);
		}

		#endregion

		#region 断言方法

		public bool Predicate(T parameter)
		{
			var predications = base.Items;

			if(predications == null || predications.Count < 1)
				return true;

			foreach(var predication in predications)
			{
				if(predication == null)
					continue;

				if(predication.Predicate(parameter))
				{
					if(_combine == PredicationCombination.Or)
						return true;
				}
				else
				{
					if(_combine == PredicationCombination.And)
						return false;
				}
			}

			return _combine == PredicationCombination.Or ? false : true;
		}

		bool IPredication.Predicate(object parameter)
		{
			T stronglyParameter;

			if(this.TryConertParameter(parameter, out stronglyParameter))
				return this.Predicate(stronglyParameter);

			return false;
		}

		#endregion
	}
}