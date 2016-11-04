using System;

namespace Tiandao.Services
{
    public interface IPredication<T> : IPredication
	{
		/// <summary>
		/// 确定指定对象是否符合某种条件。
		/// </summary>
		/// <param name="parameter">指定的条件参数对象。</param>
		/// <returns>如果符合某种条件则返回真(true)，否则返回假(false)。</returns>
		bool Predicate(T parameter);
	}
}