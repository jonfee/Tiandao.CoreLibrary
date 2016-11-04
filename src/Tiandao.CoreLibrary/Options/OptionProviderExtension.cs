using System;
using System.Reflection;

namespace Tiandao.Options
{
    public static class OptionProviderExtension
    {
		#region 公共扩展

		public static T GetOptionObject<T>(this IOptionProvider optionProvider) where T : class
		{
			if(optionProvider == null)
				throw new ArgumentNullException(nameof(optionProvider));

			string path = GetOptionPath(typeof(T));

			if(string.IsNullOrWhiteSpace(path))
				return default(T);
			else
				return optionProvider.GetOptionObject(path) as T;
		}

		public static void SetOptionObject<T>(this IOptionProvider optionProvider, object optionObject) where T : class
		{
			if(optionProvider == null)
				throw new ArgumentNullException(nameof(optionProvider));

			string path = GetOptionPath(typeof(T));

			if(string.IsNullOrWhiteSpace(path))
				throw new InvalidOperationException(string.Format("Invalid generic type '{0}'.", typeof(T).AssemblyQualifiedName));

			optionProvider.SetOptionObject(path, optionObject);
		}
		#endregion

		#region 内部方法
		internal static string GetOptionPath(Type type)
		{
			if(type == null)
				return null;

			var field = type.GetField("Path", (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static));
			if(field != null && field.FieldType == typeof(string))
				return (string)field.GetValue(null);

			var property = type.GetProperty("Path", (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static));
			if(property != null && property.CanRead && property.PropertyType == typeof(string))
				return (string)property.GetValue(null, null);

			return null;
		}

		#endregion
	}
}