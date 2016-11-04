using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace Tiandao.Options
{
    internal static class OptionHelper
    {
		#region 私有变量

		private static readonly Dictionary<string, Dictionary<string, object>> _options = new Dictionary<string, Dictionary<string, object>>(StringComparer.OrdinalIgnoreCase);
		
		#endregion

		#region 公共方法

		public static void UpdateOptionObject(string path, object optionObject)
		{
			if(string.IsNullOrWhiteSpace(path) || optionObject == null || optionObject.GetType().GetTypeInfo().IsValueType)
				return;

			Dictionary<string, object> optionData;

			if(_options.TryGetValue(path, out optionData))
			{
				string[] keys = new string[optionData.Count];
				optionData.Keys.CopyTo(keys, 0);

				foreach(var key in keys)
				{
					var property = optionObject.GetType().GetProperty(key, BindingFlags.Instance | BindingFlags.Public);

					if(property != null)
						optionData[key] = property.GetValue(optionObject, null);
				}
			}
			else
			{
				var properties = optionObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
				optionData = new Dictionary<string, object>(properties.Length);

				foreach(var property in properties)
				{
					if(property.CanRead && property.CanWrite && property.GetIndexParameters().Length == 0)
					{
						optionData[property.Name] = property.GetValue(optionObject, null);
					}
				}

				lock (((ICollection)_options).SyncRoot)
				{
					_options[path] = optionData;
				}
			}
		}

		public static void RejectOptionObject(string path, object optionObject)
		{
			if(string.IsNullOrWhiteSpace(path) || optionObject == null || optionObject.GetType().GetTypeInfo().IsValueType)
				return;

			Dictionary<string, object> optionData;

			if(_options.TryGetValue(path, out optionData))
			{
				foreach(var entry in optionData)
				{
					var property = optionObject.GetType().GetProperty(entry.Key, BindingFlags.Instance | BindingFlags.Public);

					if(property != null && property.CanWrite)
						property.SetValue(optionObject, entry.Value, null);
				}
			}
		}

		#endregion
	}
}