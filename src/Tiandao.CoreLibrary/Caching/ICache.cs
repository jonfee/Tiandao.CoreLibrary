﻿using System;
using System.Collections.Generic;

namespace Tiandao.Caching
{
    public interface ICache
    {
		/// <summary>
		/// 表示缓存发生改变的事件。
		/// </summary>
		event EventHandler<CacheChangedEventArgs> Changed;

		/// <summary>
		/// 获取当前缓存容器的名字。
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// 获取一个值，表示缓存字典内的记录总数。
		/// </summary>
		long Count
		{
			get;
		}

		/// <summary>
		/// 获取或设置一个缓存项的创建器。
		/// </summary>
		ICacheCreator Creator
		{
			get;
			set;
		}

		/// <summary>
		/// 检测指定键的缓存项是否存在。
		/// </summary>
		/// <param name="key">指定要检测的键。</param>
		/// <returns>如果存在则返回真(True)，否则返回假(False)。</returns>
		bool Exists(string key);

		/// <summary>
		/// 获取指定键的缓存项的剩下的生存时长。
		/// </summary>
		/// <param name="key">指定要设置的键。</param>
		/// <returns>返回指定缓存项的生成时长，如果为空则表示该缓存项为永久缓存项。</returns>
		TimeSpan? GetDuration(string key);

		/// <summary>
		/// 设置指定键的缓存项的生存时长。
		/// </summary>
		/// <param name="key">指定要设置的键。</param>
		/// <param name="duration">指定要设置的生存时长，如果为零则将该缓存项设置成永不过期。</param>
		void SetDuration(string key, TimeSpan duration);

		/// <summary>
		/// 从缓存字典中获取指定键的缓存值。
		/// </summary>
		/// <param name="key">指定要获取的键。</param>
		/// <returns>如果指定的键存在则返回对应的值，如果不存在则通过<seealso cref="Creator"/>属性指定的创建器去创建一个缓存项，并将该新建的缓存项保存并返回；如果<seealso cref="Creator"/>属性值为空，则返回<typeparam name="T" />泛型的默认值。</returns>
		T GetValue<T>(string key);

		/// <summary>
		/// 从缓存字典中获取指定键的缓存值。
		/// </summary>
		/// <param name="key">指定要获取的键。</param>
		/// <returns>如果指定的键存在则返回对应的值，如果不存在则通过<seealso cref="Creator"/>属性指定的创建器去创建一个缓存项，并将该新建的缓存项保存并返回；如果<seealso cref="Creator"/>属性值为空，则返回空(null)。</returns>
		object GetValue(string key);

		/// <summary>
		/// 从缓存字典中获取指定键的缓存值。
		/// </summary>
		/// <param name="key">指定要获取的键。</param>
		/// <param name="valueCreator">如果指定的键不存在则使用该委托进行创建它，如果该参数为空则当指定的键不存在时返回空(null)。</param>
		/// <returns>返回指定键的值或新增并保存的缓存项的值。</returns>
		object GetValue(string key, Func<string, Tuple<object, DateTime>> valueCreator);

		/// <summary>
		/// 从缓存字典中获取指定键的缓存值。
		/// </summary>
		/// <param name="key">指定要获取的键。</param>
		/// <param name="valueCreator">如果指定的键不存在则使用该委托进行创建它，如果该参数为空则当指定的键不存在时返回空(null)。</param>
		/// <returns>返回指定键的值或新增并保存的缓存项的值。</returns>
		object GetValue(string key, Func<string, Tuple<object, TimeSpan>> valueCreator);

		/// <summary>
		/// 设置指定的值保存到缓存字典中。
		/// </summary>
		/// <param name="key">指定要保存的键。</param>
		/// <param name="value">指定要保存的值。</param>
		/// <returns>如果设置成功则返回真(true)，否则返回假(false)。</returns>
		bool SetValue(string key, object value);

		/// <summary>
		/// 设置指定的值保存到缓存字典中。
		/// </summary>
		/// <param name="key">指定要保存的键。</param>
		/// <param name="value">指定要保存的值。</param>
		/// <param name="duration">指定缓存项的生存时长，如果为零则表示永不过期。</param>
		/// <param name="requiredNotExists">设置一个值，当指定的<paramref name="key"/>是不存在的则设置该缓存项，否则不执行任何动作。</param>
		/// <returns>如果设置成功则返回真(true)，否则返回假(false)。</returns>
		bool SetValue(string key, object value, TimeSpan duration, bool requiredNotExists = false);

		/// <summary>
		/// 设置指定的值保存到缓存字典中。
		/// </summary>
		/// <param name="key">指定要保存的键。</param>
		/// <param name="value">指定要保存的值。</param>
		/// <param name="expires">指定缓存项的过期时间，如果小于当前时间则表示永不过期。</param>
		/// <param name="requiredNotExists">设置一个值，当指定的<paramref name="key"/>是不存在的则设置该缓存项，否则不执行任何动作。</param>
		/// <returns>如果设置成功则返回真(true)，否则返回假(false)。</returns>
		bool SetValue(string key, object value, DateTime expires, bool requiredNotExists = false);

		/// <summary>
		/// 修改指定键的缓存项的键名。
		/// </summary>
		/// <param name="key">指定要更名的键。</param>
		/// <param name="newKey">要更改的新键。</param>
		/// <returns>如果设置成功则返回真(True)，否则返回假(False)。</returns>
		bool Rename(string key, string newKey);

		/// <summary>
		/// 从缓存字典中删除指定键的缓存项。
		/// </summary>
		/// <param name="key">指定要删除的键。</param>
		/// <returns>如果指定的键存在则返回真(True)，否则返回假(False)。</returns>
		bool Remove(string key);

		/// <summary>
		/// 清空缓存字典中的所有数据。
		/// </summary>
		void Clear();
	}
}