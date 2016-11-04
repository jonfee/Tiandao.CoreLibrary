using System;

namespace Tiandao.Services
{
	/// <summary>
	/// 提供一种特定于类型的通用匹配方法，某些同类型的类通过实现此接口对其进行更进一步的匹配。
	/// </summary>
	public interface IMatchable
    {
		/// <summary>
		/// 指示当前对象是否匹配指定参数的条件约束。
		/// </summary>
		/// <param name="parameter">指定是否匹配逻辑的参数。</param>
		/// <returns>如果当前对象符合<paramref name="parameter"/>参数的匹配规则，则为真(true)；否则为假(false)。</returns>
		bool IsMatch(object parameter);
	}
}