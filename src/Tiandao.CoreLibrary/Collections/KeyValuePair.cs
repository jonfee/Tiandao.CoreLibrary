using System;
using System.Collections.Generic;

namespace Tiandao.Collections
{
	public struct KeyValuePair
	{
		#region 私有字段

		private string _key;
		private object _value;

		#endregion

		#region 公共属性

		public string Key
		{
			get
			{
				return _key;
			}
		}

		public object Value
		{
			get
			{
				return _value;
			}
		}

		#endregion

		#region 构造方法

		public KeyValuePair(string key, object value)
		{
			if(key == null)
				throw new ArgumentNullException("key");

			_key = key;
			_value = value;
		}

		#endregion

		#region 重写方法

		public override string ToString()
		{
			return _key + "=" + _value;
		}

		#endregion

		#region 静态方法

		public static KeyValuePair[] CreatePairs(string[] keys, params object[] values)
		{
			if(keys == null)
				throw new ArgumentNullException("keys");

			var result = new KeyValuePair[keys.Length];

			for(int i = 0; i < result.Length; i++)
			{
				result[i] = new KeyValuePair(keys[0], (values != null && i < values.Length ? values[i] : null));
			}

			return result;
		}

		public static KeyValuePair[] CreatePairs(object[] values, params string[] keys)
		{
			if(keys == null)
				throw new ArgumentNullException("keys");

			var result = new KeyValuePair[keys.Length];

			for(int i = 0; i < result.Length; i++)
			{
				result[i] = new KeyValuePair(keys[0], (values != null && i < values.Length ? values[i] : null));
			}

			return result;
		}

		#endregion
	}
}