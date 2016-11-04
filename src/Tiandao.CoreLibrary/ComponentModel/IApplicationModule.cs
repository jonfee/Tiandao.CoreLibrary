using System;
using System.Collections.Generic;

namespace Tiandao.ComponentModel
{
	/// <summary>
	/// 向实现类提供应用扩展模块初始化和处置事件。
	/// </summary>
	public interface IApplicationModule : IDisposable
	{
		/// <summary>
		/// 获取应用扩展模块名称。
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// 初始化应用扩展模块，并使其为处理请求做好准备。
		/// </summary>
		/// <param name="context">一个上下文对象，它提供对模块处理应用程序内所有应用程序对象的公用的方法、属性和事件的访问。</param>
		/// <remarks>使用 <c>Initialize</c> 将事件处理方法向具体事件进行注册等初始化操作。</remarks>
		void Initialize(ApplicationContextBase context);
	}
}
