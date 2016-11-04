using System;
using System.Collections.Generic;

namespace Tiandao.Options
{
	/// <summary>
	/// 提供选项数据的获取与保存。
	/// </summary>
	public interface IOptionProvider
    {
		/// <summary>
		/// 根据指定的选项路径获取对应的选项数据。
		/// </summary>
		/// <param name="path">要获取的选项路径。</param>
		/// <returns>获取到的选项数据对象。</returns>
		object GetOptionObject(string path);

		/// <summary>
		/// 将指定的选项数据保存到指定路径的存储容器中。
		/// </summary>
		/// <param name="path">待保存的选项路径。</param>
		/// <param name="optionObject">待保存的选项对象。</param>
		void SetOptionObject(string path, object optionObject);
	}
}