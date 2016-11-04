using System;
using System.Collections.Generic;

namespace Tiandao.Options
{
    public interface ISettingsProvider
    {
		/// <summary>
		/// 获取指定名称的自定义设置项的值。
		/// </summary>
		/// <param name="name">要查找的自定义设置项的名称。</param>
		/// <returns>返回指定名称的自定义设置项的值。如果指定名称的设置项不存在则返回空(null)。</returns>
		/// <remarks>
		///		<para>实现要求：如果指定名称的设置项不存在或查找失败，不要抛出异常，应返回空(null)。</para>
		/// </remarks>
		object GetValue(string name);

		void SetValue(string name, object value);
	}
}