using System;
using System.Collections.Generic;

namespace Tiandao.Caching
{
	/// <summary>
	/// 表示缓存容器的提供程序的接口。
	/// </summary>
	public interface ICacheProvider
    {
		/// <summary>
		/// 获取指定名称的缓存容器。
		/// </summary>
		/// <param name="name">指定要获取的缓存容器的名称，如果为空(null)或空字符串则返回默认缓存容器。</param>
		/// <param name="createNotExists">指示如果指定名称的缓存容器不存在时是否要自动创建它。</param>
		/// <returns>
		///		<para>返回指定名称的缓存容。</para>
		///		<para>如果指定名称的缓存容器不存在并且<paramref name="createNotExists"/>参数为假(False)则返回空(null)；</para>
		///		<para>如果指定名称的缓存容器不存在并且<paramref name="createNotExists"/>参数为真(True)则创建一个指定名称的缓存容器并返回它。</para>
		///	</returns>
		ICache GetCache(string name, bool createNotExists = false);
	}
}