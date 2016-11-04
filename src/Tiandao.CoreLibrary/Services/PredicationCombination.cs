using System;

namespace Tiandao.Services
{
    public enum PredicationCombination
    {
		/// <summary>
		/// 表示如果某个断言返回成功，则不再执行后续断言测试而直接返回成功；如果返回失败，则进行后续断言测试。即整个断言链中所有断言测试均失败则断言链返回失败。
		/// </summary>
		Or,

		/// <summary>
		/// 表示如果某个断言返回成功，则进行下一个断言测试，如果返回失败，则不再执行后续断言测试而直接返回失败。即整个断言链中所有断言测试均成功则断言链返回成功。
		/// </summary>
		And,
	}
}
