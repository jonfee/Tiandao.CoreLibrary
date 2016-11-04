using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
	/// <summary>
	/// 提供关于服务供应程序容器的功能。
	/// </summary>
	public interface IServiceProviderFactory
    {
		/// <summary>
		/// 获取或设置默认的服务供应程序。
		/// </summary>
		IServiceProvider Default
		{
			get;
		}

		/// <summary>
		/// 获取指定名称的服务供应程序。
		/// </summary>
		/// <param name="name">指定的服务供应程序名称。</param>
		/// <returns>返回指定名称的服务供应程序。</returns>
		IServiceProvider GetProvider(string name);
	}
}
