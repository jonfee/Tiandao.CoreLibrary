using System;
using System.Collections.Generic;

namespace Tiandao.Common
{
    public static class ObjectExtension
    {
		#region 类型转换

		public static T ConvertTo<T>(this object value)
	    {
		    return Converter.ConvertValue<T>(value);
	    }

	    public static T ConvertTo<T>(this object value, T defaultValue)
	    {
		    return Converter.ConvertValue<T>(value, defaultValue);
	    }

	    public static T ConvertTo<T>(this object value, Func<object> defaultValueThunk)
	    {
			return Converter.ConvertValue<T>(value, defaultValueThunk);
		}

	    public static object ConvertTo(this object value, Type conversionType)
	    {
			return Converter.ConvertValue(value, conversionType);
		}

	    public static object ConvertTo(this object value, Type conversionType, object defaultValue)
	    {
			return Converter.ConvertValue(value, conversionType, defaultValue);
		}

	    public static object ConvertTo(this object value, Type conversionType, Func<object> defaultValueThunk)
	    {
			return Converter.ConvertValue(value, conversionType, defaultValueThunk);
		}

	    public static bool TryConvertTo<T>(this object value, out T result)
	    {
		    return Converter.TryConvertValue<T>(value, out result);
	    }

	    public static bool TryConvertTo(object value, Type conversionType, out object result)
	    {
		    return Converter.TryConvertValue(value, conversionType, out result);
	    }

		#endregion

		#region 对象解析

	    public static Type GetMemberType(this object target, string text)
	    {
		    return Converter.GetMemberType(target, text);
	    }

	    public static bool TryGetMemberType(this object target, string text, out Type memberType)
	    {
		    return Converter.TryGetMemberType(target, text, out memberType);
	    }

	    public static object GetValue(this object target, string text)
	    {
		    return Converter.GetValue(target, text);
	    }

	    public static object GetValue(this object target, string[] memberNames)
	    {
		    return Converter.GetValue(target, memberNames);
	    }

	    public static void SetValue(this object target, string text, object value)
	    {
		    Converter.SetValue(target, text, value);
	    }

	    public static void SetValue(this object target, string[] memberNames, object value)
	    {
			Converter.SetValue(target, memberNames, value);
	    }

	    #endregion
	}
}