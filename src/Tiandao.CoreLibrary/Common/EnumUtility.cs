using System;
using System.Reflection;
using System.Linq;

using Tiandao.ComponentModel;

namespace Tiandao.Common
{
	/// <summary>
	/// 为 <see cref="System.Enum"/> 类型扩展的辅助类。
	/// </summary>
	public static class EnumUtility
    {
		public static string Format(object value, string format)
		{
			if(value == null)
				return string.Empty;

			var enumType = GetEnumType(value.GetType());

			if(enumType != null)
				return GetEnumEntry((Enum)value).ToString(format);

			if(string.IsNullOrWhiteSpace(format))
				return string.Format("{0}", value);
			else
				return string.Format("{0:" + format + "}", value);
		}

		public static Type GetEnumType(Type type)
		{
			if(type == null)
				return null;

			if(type.IsEnum())
				return type;

			if(type.IsGenericType() && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GetGenericArguments()[0].GetTypeInfo().IsEnum)
				return type.GetGenericArguments()[0];

			return null;
		}

		/// <summary>
		/// 获取指定枚举项对应的 <see cref="EnumEntry"/> 描述对象。
		/// </summary>
		/// <param name="enumValue">要获取的枚举项。</param>
		/// <returns>返回指定枚举值对应的<seealso cref="EnumEntry"/>对象。</returns>
		public static EnumEntry GetEnumEntry(this Enum enumValue)
		{
			return GetEnumEntry(enumValue, false);
		}

		/// <summary>
		/// 获取指定枚举项对应的 <see cref="EnumEntry"/> 描述对象。 
		/// </summary>
		/// <param name="enumValue">要获取的枚举项。</param>
		/// <param name="underlyingType">是否将生成的 <seealso cref="EnumEntry"/> 元素的 <seealso cref="EnumEntry.Value"/> 属性值置为 enumType 参数对应的枚举项基类型值。</param>
		/// <returns>返回指定枚举值对应的 <seealso cref="EnumEntry"/> 对象。</returns>
		public static EnumEntry GetEnumEntry(this Enum enumValue, bool underlyingType)
		{
			FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());
			var alias = field.GetCustomAttributes(typeof(AliasAttribute), false).OfType<AliasAttribute>().FirstOrDefault();

			var description = field.GetCustomAttributes(typeof(DescriptionAttribute), false).OfType<DescriptionAttribute>().FirstOrDefault();

			return new EnumEntry(enumValue.GetType(), field.Name,
								underlyingType ? System.Convert.ChangeType(field.GetValue(null), Enum.GetUnderlyingType(enumValue.GetType())) : field.GetValue(null),
								alias == null ? string.Empty : alias.Alias,
								description == null ? string.Empty : Resources.ResourceUtility.GetString(description.Description, enumValue.GetType().GetAssembly()));
		}

		/// <summary>
		/// 获取指定枚举的描述对象数组。
		/// </summary>
		/// <param name="enumType">要获取的枚举类型。</param>
		/// <param name="underlyingType">是否将生成的 <seealso cref="EnumEntry"/> 元素的 <seealso cref="EnumEntry.Value"/> 属性值置为 enumType 参数对应的枚举项基类型值。</param>
		/// <returns>返回的枚举描述对象数组。</returns>
		public static EnumEntry[] GetEnumEntries(Type enumType, bool underlyingType)
		{
			return GetEnumEntries(enumType, underlyingType, null, string.Empty);
		}

		/// <summary>
		/// 获取指定枚举的描述对象数组。
		/// </summary>
		/// <param name="enumType">要获取的枚举类型，可为<seealso cref="System.Nullable"/>类型。</param>
		/// <param name="underlyingType">是否将生成的 <seealso cref="EnumEntry"/> 元素的 <seealso cref="EnumEntry.Value"/> 属性值置为 enumType 参数对应的枚举项基类型值。</param>
		/// <param name="nullValue">如果参数<paramref name="enumType"/>为可空类型时，该空值对应的<seealso cref="EnumEntry.Value"/>属性的值。</param>
		/// <returns>返回的枚举描述对象数组。</returns>
		public static EnumEntry[] GetEnumEntries(Type enumType, bool underlyingType, object nullValue)
		{
			return GetEnumEntries(enumType, underlyingType, nullValue, string.Empty);
		}

		/// <summary>
		/// 获取指定枚举的描述对象数组。
		/// </summary>
		/// <param name="enumType">要获取的枚举类型，可为<seealso cref="System.Nullable"/>类型。</param>
		/// <param name="underlyingType">是否将生成的 <seealso cref="EnumEntry"/> 元素的 <seealso cref="EnumEntry.Value"/> 属性值置为 enumType 参数对应的枚举项基类型值。</param>
		/// <param name="nullValue">如果参数<paramref name="enumType"/>为可空类型时，该空值对应的<seealso cref="EnumEntry.Value"/>属性的值。</param>
		/// <param name="nullText">如果参数<paramref name="enumType"/>为可空类型时，该空值对应的<seealso cref="EnumEntry.Description"/>属性的值。</param>
		/// <returns>返回的枚举描述对象数组。</returns>
		public static EnumEntry[] GetEnumEntries(Type enumType, bool underlyingType, object nullValue, string nullText)
		{
			if(enumType == null)
				throw new ArgumentNullException(nameof(enumType));

			Type underlyingTypeOfNullable = Nullable.GetUnderlyingType(enumType);
			if(underlyingTypeOfNullable != null)
				enumType = underlyingTypeOfNullable;

			EnumEntry[] entries;
			int baseIndex = (underlyingTypeOfNullable == null) ? 0 : 1;
			var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);

			if(underlyingTypeOfNullable == null)
			{
				entries = new EnumEntry[fields.Length];
			}
			else
			{
				entries = new EnumEntry[fields.Length + 1];
				entries[0] = new EnumEntry(enumType, string.Empty, nullValue, nullText, nullText);
			}

			for(int i = 0; i < fields.Length; i++)
			{
				var alias = fields[i].GetCustomAttributes(typeof(AliasAttribute), false).OfType<AliasAttribute>().FirstOrDefault();
				var description = fields[i].GetCustomAttributes(typeof(DescriptionAttribute), false).OfType<DescriptionAttribute>().FirstOrDefault();

				entries[baseIndex + i] = new EnumEntry(enumType, fields[i].Name,
													underlyingType ? System.Convert.ChangeType(fields[i].GetValue(null), Enum.GetUnderlyingType(enumType)) : fields[i].GetValue(null),
													alias == null ? string.Empty : alias.Alias,
													description == null ? string.Empty : Resources.ResourceUtility.GetString(description.Description, enumType.GetAssembly()));
			}

			return entries;
		}
	}
}