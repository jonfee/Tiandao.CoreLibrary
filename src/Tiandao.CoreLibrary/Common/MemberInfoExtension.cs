using System;
using System.Reflection;

namespace Tiandao.Common
{
    public static class MemberInfoExtension
    {
	    public static bool IsField(this MemberInfo member)
	    {
#if !CORE_CLR
			return member.MemberType == MemberTypes.Field;
#else
		    return member is FieldInfo;
#endif
		}

	    public static bool IsProperty(this MemberInfo member)
	    {
#if !CORE_CLR
			return member.MemberType == MemberTypes.Property;
#else
		    return member is PropertyInfo;
#endif
		}

		public static bool IsMethod(this MemberInfo member)
	    {
#if !CORE_CLR
			return member.MemberType == MemberTypes.Method;
#else
		    return member is MethodInfo;
#endif
		}
	}
}