using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tiandao.Common
{
	/// <summary>
	/// 基于 <see cref="System.Type"/> 的实用扩展类。
	/// </summary>
	public static class TypeExtension
	{
		/// <summary>
		/// 提供比 <see cref="System.Type.IsAssignableFrom"/> 加强的功能，支持对泛型定义接口或类的匹配。
		/// </summary>
		/// <param name="type">指定的接口或基类的类型。</param>
		/// <param name="instanceType">指定的实例类型。</param>
		/// <returns>如果当满足如下条件之一则返回真(true)：
		/// <list type="bullet">
		///		<item>
		///			<term>如果 <paramref name="type"/> 为泛型定义类型，则 <paramref name="instanceType"/> 实现的接口或基类中有从 <paramref name="type"/> 指定的泛型定义中泛化的版本。</term>
		///		</item>
		///		<item>
		///			<term>如果 <paramref name="type"/> 和当前 <paramref name="instanceType"/> 表示同一类型；</term>
		///		</item>
		///		<item>
		///			<term>当前 <paramref name="instanceType"/> 位于 <paramref name="type"/> 的继承层次结构中；</term>
		///		</item>
		///		<item>
		///			<term>当前 <paramref name="instanceType"/> 是 <paramref name="type"/> 实现的接口；</term>
		///		</item>
		///		<item>
		///			<term><paramref name="type"/> 是泛型类型参数且当前 <paramref name="instanceType"/> 表示 <paramref name="type"/> 的约束之一。</term>
		///		</item>
		/// </list>
		/// </returns>
		/// <remarks>
		///		<para>除了 <see cref="System.Type.IsAssignableFrom"/> 支持的特性外，增加了如下特性：</para>
		///		<example>
		///		<code>
		///		TypeExtension.IsAssignableFrom(typeof(IDictionary&lt;,&gt;), typeof(IDictionary&lt;string, object&gt;));	// true
		///		TypeExtension.IsAssignableFrom(typeof(IDictionary&lt;,&gt;), typeof(Dictionary&lt;string, object&gt;));	// true
		///		TypeExtension.IsAssignableFrom(typeof(Dictionary&lt;,&gt;), typeof(Dictioanry&lt;string, int&gt;));		//true
		///		
		///		public class MyNamedCollection&lt;T&gt; : Collection&lt;T&gt;, IDictionary&lt;string, T&gt;
		///		{
		///		}
		///		
		///		TypeExtension.IsAssignableFrom(typeof(IDictionary&lt;,&gt;), typeof(MyNamedCollection&lt;string, object&gt;)); //true
		///		</code>
		///		</example>
		/// </remarks>
		public static bool IsAssignableFrom(this Type type, Type instanceType)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			if(instanceType == null)
				throw new ArgumentNullException("instanceType");

			if(type.IsGenericType() && type.IsGenericTypeDefinition())
			{
				IEnumerable<Type> baseTypes = null;

				if(type.IsInterface())
				{
					if(instanceType.IsInterface())
					{
						baseTypes = new List<Type>(new Type[] { instanceType });
						((List<Type>)baseTypes).AddRange(instanceType.GetInterfaces());
					}
					else
					{
						baseTypes = instanceType.GetInterfaces();
					}
				}
				else
				{
					baseTypes = new List<Type>();

					var currentType = instanceType;

					while(currentType != typeof(object) &&
						  currentType != typeof(Enum) &&
						  currentType != typeof(Delegate) &&
						  currentType != typeof(ValueType))
					{
						((List<Type>)baseTypes).Add(currentType);
						currentType = currentType.GetBaseType();
					}
				}

				foreach(var baseType in baseTypes)
				{
					if(baseType.IsGenericType() && baseType.GetGenericTypeDefinition() == type)
					{
						return true;
					}
				}
			}

#if !CORE_CLR
			return type.IsAssignableFrom(instanceType);
#else
			return TypeExtensions.IsAssignableFrom(type, instanceType);
#endif
		}

		/// <summary>
		/// 获取一个值，通过该值指示 <paramref name="type"/> 是否为基元类型之一。 
		/// </summary>
		/// <param name="type">需要检测的 <see cref="System.Type"/> 实例。</param>
		/// <returns>如果 Type 为基元类型之一，则为 true；否则为 false。</returns>
		public static bool IsScalarType(this Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			if(type.IsArray)
				return IsScalarType(type.GetElementType());

			if(type.IsGenericType() && type.GetGenericTypeDefinition() == typeof(Nullable<>))
				return IsScalarType(type.GetGenericArguments()[0]);

			var result = type.IsPrimitive() || type.IsEnum() ||
			 type == typeof(string) || type == typeof(decimal) ||
			 type == typeof(DateTime) || type == typeof(TimeSpan) ||
			 type == typeof(DateTimeOffset) || type == typeof(Guid);

			if(result)
				return result;

			var converter = TypeDescriptor.GetConverter(type);

			return (converter != null && converter.CanConvertFrom(typeof(string)) && converter.CanConvertTo(typeof(string)));
		}

		public static bool IsInteger(this Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			return type == typeof(Byte) || type == typeof(SByte) ||
				   type == typeof(Int16) || type == typeof(UInt16) ||
				   type == typeof(Int32) || type == typeof(UInt32) ||
				   type == typeof(Int64) || type == typeof(UInt64);
		}

		public static bool IsNumber(this Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			return TypeExtension.IsInteger(type) ||
				   type == typeof(Single) || type == typeof(Double) ||
				   type == typeof(Decimal) || type == typeof(Char);

		}

		/// <summary>
		/// 获取具有指定名称的 <see cref="System.Type"/>，指定是否执行区分大小写的搜索，以及在找不到类型时是否引发异常。 
		/// </summary>
		/// <param name="typeName">要获取的类型的程序集限定名称。</param>
		/// <param name="throwOnError">如果为 true，则在找不到该类型时引发异常；如果为 false，则返回 null。</param>
		/// <param name="ignoreCase">为 <paramref name="typeName"/> 执行的搜索不区分大小写则为 true，为 <paramref name="typeName"/> 执行的搜索区分大小写则为 false。 </param>
		/// <returns>具有指定名称的 <see cref="System.Type"/>（如果找到的话）；否则为 null。</returns>
		public static Type GetType(string typeName, bool throwOnError = false, bool ignoreCase = true)
		{
			if(string.IsNullOrWhiteSpace(typeName))
			{
				return null;
			}

			typeName = typeName.Replace(" ", "");

			switch(typeName.ToLowerInvariant())
			{
				case "string":
					return typeof(string);
				case "string[]":
					return typeof(string[]);

				case "int":
					return typeof(int);
				case "int?":
					return typeof(int?);
				case "int[]":
					return typeof(int[]);

				case "long":
					return typeof(long);
				case "long?":
					return typeof(long?);
				case "long[]":
					return typeof(long[]);

				case "short":
					return typeof(short);
				case "short?":
					return typeof(short?);
				case "short[]":
					return typeof(short[]);

				case "byte":
					return typeof(byte);
				case "byte?":
					return typeof(byte?);
				case "binary":
				case "byte[]":
					return typeof(byte[]);

				case "bool":
				case "boolean":
					return typeof(bool);
				case "bool?":
				case "boolean?":
					return typeof(bool?);
				case "bool[]":
				case "boolean[]":
					return typeof(bool[]);

				case "money":
				case "currency":
				case "decimal":
					return typeof(decimal);
				case "money?":
				case "currency?":
				case "decimal?":
					return typeof(decimal?);
				case "money[]":
				case "currency[]":
				case "decimal[]":
					return typeof(decimal[]);

				case "float":
				case "single":
					return typeof(float);
				case "float?":
				case "single?":
					return typeof(float?);
				case "float[]":
				case "single[]":
					return typeof(float[]);

				case "double":
				case "number":
					return typeof(double);
				case "double?":
				case "number?":
					return typeof(double?);
				case "double[]":
				case "number[]":
					return typeof(double[]);

				case "uint":
					return typeof(uint);
				case "uint?":
					return typeof(uint?);
				case "uint[]":
					return typeof(uint[]);

				case "ulong":
					return typeof(ulong);
				case "ulong?":
					return typeof(ulong?);
				case "ulong[]":
					return typeof(ulong[]);

				case "ushort":
					return typeof(ushort);
				case "ushort?":
					return typeof(ushort?);
				case "ushort[]":
					return typeof(ushort[]);

				case "sbyte":
					return typeof(sbyte);
				case "sbyte?":
					return typeof(sbyte?);
				case "sbyte[]":
					return typeof(sbyte[]);

				case "char":
					return typeof(char);
				case "char?":
					return typeof(char?);
				case "char[]":
					return typeof(char[]);

				case "date":
				case "time":
				case "datetime":
					return typeof(DateTime);
				case "date?":
				case "time?":
				case "datetime?":
					return typeof(DateTime?);
				case "date[]":
				case "time[]":
				case "datetime[]":
					return typeof(DateTime[]);

				case "timespan":
					return typeof(TimeSpan);
				case "timespan?":
					return typeof(TimeSpan?);
				case "timespan[]":
					return typeof(TimeSpan[]);

				case "guid":
					return typeof(Guid);
				case "guid?":
					return typeof(Guid?);
				case "guid[]":
					return typeof(Guid[]);

				case "object":
					return typeof(object);
				case "void":
					return typeof(void);
			}

			if(!typeName.Contains("."))
			{
				typeName = "System." + typeName;
			}

			return Type.GetType(typeName, throwOnError, ignoreCase);
		}

		/// <summary>
		/// 获取一个值，通过该值指示 type 是否为 Nullable 类型。
		/// </summary>
		/// <param name="type">类型实例。</param>
		/// <returns>一个 bool 值。</returns>
		public static bool IsNullable(this Type type)
		{
			return Nullable.GetUnderlyingType(type) != null;
		}

		/// <summary>
		/// 是否是具体类型，凡是能直接实例化的类型都是具体类型。
		/// </summary>
		public static bool IsConcreteType(this Type type)
		{
			if(type == null)
			{
				throw new ArgumentNullException("type");
			}

			return !type.IsGenericTypeDefinition() && !type.IsAbstract() && !type.IsInterface();
		}

		public static bool IsClass(this Type type)
		{
#if !CORE_CLR
			return type.IsClass;
#else
			return type.GetTypeInfo().IsClass;
#endif
		}

		public static bool IsInterface(this Type type)
		{
#if !CORE_CLR
			return type.IsInterface;
#else
			return type.GetTypeInfo().IsInterface;
#endif
		}

		public static bool IsAbstract(this Type type)
		{
#if !CORE_CLR
			return type.IsAbstract;
#else
			return type.GetTypeInfo().IsAbstract;
#endif
		}

		public static bool IsEnum(this Type type)
		{
#if !CORE_CLR
			return type.IsEnum;
#else
			return type.GetTypeInfo().IsEnum;
#endif
		}

		public static bool IsPrimitive(this Type type)
		{
#if !CORE_CLR
			return type.IsPrimitive;
#else
			return type.GetTypeInfo().IsPrimitive;
#endif
		}

		public static bool IsGenericType(this Type type)
		{
#if !CORE_CLR
			return type.IsGenericType;
#else
			return type.GetTypeInfo().IsGenericType;
#endif
		}

		public static bool IsGenericTypeDefinition(this Type type)
		{
#if !CORE_CLR
			return type.IsGenericTypeDefinition;
#else
			return type.GetTypeInfo().IsGenericTypeDefinition;
#endif
		}

		public static bool IsValueType(this Type type)
		{
#if !CORE_CLR
			return type.IsValueType;
#else
			return type.GetTypeInfo().IsValueType;
#endif
		}

		public static bool IsSubclassOf(this Type type, Type c)
		{
#if !CORE_CLR
			return type.IsSubclassOf(c);
#else
			return type.GetTypeInfo().IsSubclassOf(c);
#endif
		}

		public static Type GetBaseType(this Type type)
		{
#if !CORE_CLR
			return type.BaseType;
#else
			return type.GetTypeInfo().BaseType;
#endif
		}

		public static Assembly GetAssembly(this Type type)
		{
#if !CORE_CLR
			return type.Assembly;
#else
			return type.GetTypeInfo().Assembly;
#endif
		}

		public static Attribute GetCustomAttribute(this Type type, Type attributeType)
		{
#if !CORE_CLR
			return Attribute.GetCustomAttribute(type, attributeType);
#else
			return type.GetTypeInfo().GetCustomAttribute(attributeType);
#endif
		}

		public static Attribute GetCustomAttribute(this Type type, Type attributeType, bool inherit)
		{
#if !CORE_CLR
			return Attribute.GetCustomAttribute(type, attributeType, inherit);
#else
			return type.GetTypeInfo().GetCustomAttribute(attributeType, inherit);
#endif
		}

		public static IEnumerable<Attribute> GetCustomAttributes(this Type type)
		{
#if !CORE_CLR
			 return Attribute.GetCustomAttributes(type);
#else
			return type.GetTypeInfo().GetCustomAttributes();
#endif
		}

		public static IEnumerable<Attribute> GetCustomAttributes(this Type type, Type attributeType, bool inherit)
		{
#if !CORE_CLR
			return Attribute.GetCustomAttributes(type, attributeType, inherit);
#else
			return type.GetTypeInfo().GetCustomAttributes(attributeType, inherit);
#endif
		}

		/// <summary>
		/// 创建 T 类型的实例。
		/// </summary>
		public static T CreateInstance<T>(this Type type, params object[] args)
		{
			if(type == null)
			{
				throw new ArgumentNullException("type");
			}

			return (T)type.CreateInstance(args);
		}

		/// <summary>
		/// 创建 type 类型的实例。
		/// </summary>
		public static object CreateInstance(this Type type, params object[] args)
		{
			if(type == null)
			{
				throw new ArgumentNullException("type");
			}

			return Activator.CreateInstance(type, args);
		}

		public static TypeCode GetTypeCode(this Type type)
		{
#if !CORE_CLR
			return Type.GetTypeCode(type);
#else
			if(type == null)
			{
				return TypeCode.Empty;
			}
			else if(type == typeof(Boolean))
			{
				return TypeCode.Boolean;
			}
			else if(type == typeof(Char))
			{
				return TypeCode.Char;
			}
			else if(type == typeof(SByte))
			{
				return TypeCode.SByte;
			}
			else if(type == typeof(Byte))
			{
				return TypeCode.Byte;
			}
			else if(type == typeof(Int16))
			{
				return TypeCode.Int16;
			}
			else if(type == typeof(UInt16))
			{
				return TypeCode.UInt16;
			}
			else if(type == typeof(Int32))
			{
				return TypeCode.Int32;
			}
			else if(type == typeof(UInt32))
			{
				return TypeCode.UInt32;
			}
			else if(type == typeof(Int64))
			{
				return TypeCode.Int64;
			}
			else if(type == typeof(UInt64))
			{
				return TypeCode.UInt64;
			}
			else if(type == typeof(Single))
			{
				return TypeCode.Single;
			}
			else if(type == typeof(Double))
			{
				return TypeCode.Double;
			}
			else if(type == typeof(Decimal))
			{
				return TypeCode.Decimal;
			}
			else if(type == typeof(DateTime))
			{
				return TypeCode.DateTime;
			}
			else if(type == typeof(String))
			{
				return TypeCode.String;
			}

			return TypeCode.Object;
#endif
		}
	}
}