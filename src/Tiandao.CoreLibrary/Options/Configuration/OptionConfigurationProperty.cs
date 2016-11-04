using System;
using System.Reflection;
using System.ComponentModel;
using Tiandao.Common;

namespace Tiandao.Options.Configuration
{
    public class OptionConfigurationProperty
    {
		#region 私有字段

		private string _name;
		private string _elementName;
		private Type _type;
		private object _defaultValue;
		private TypeConverter _converter;
		private OptionConfigurationPropertyBehavior _behavior;

		#endregion

		#region 公共属性

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public string ElementName
		{
			get
			{
				return _elementName;
			}
		}

		public Type Type
		{
			get
			{
				return _type;
			}
		}

		public object DefaultValue
		{
			get
			{
				return _defaultValue;
			}
			set
			{
				if(value == null)
				{
					if(_type.GetTypeInfo().IsValueType)
						_defaultValue = Activator.CreateInstance(_type);
					else
						_defaultValue = null;
				}
				else
				{
					if(_converter != null && _converter.CanConvertFrom(value.GetType()))
						_defaultValue = _converter.ConvertFrom(value);
					else
						_defaultValue = Common.Converter.ConvertValue(value, _type);
				}
			}
		}

		public TypeConverter Converter
		{
			get
			{
				return _converter;
			}
		}

		public OptionConfigurationPropertyBehavior Behavior
		{
			get
			{
				return _behavior;
			}
		}

		public bool IsKey
		{
			get
			{
				return ((int)_behavior & 2) == 2;
			}
		}

		public bool IsRequired
		{
			get
			{
				return (_behavior & OptionConfigurationPropertyBehavior.IsRequired) == OptionConfigurationPropertyBehavior.IsRequired;
			}
		}

		public bool IsDefaultCollection
		{
			get
			{
				return string.IsNullOrEmpty(_name) && this.IsCollection;
			}
		}

		public bool IsCollection
		{
			get
			{
				return TypeExtension.IsAssignableFrom(typeof(OptionConfigurationElementCollection), _type);
			}
		}

		#endregion

		#region 构造方法

		public OptionConfigurationProperty(string name, Type type) : this(name, type, null, OptionConfigurationPropertyBehavior.None, null)
		{

		}

		public OptionConfigurationProperty(string name, string elementName, Type type) : this(name, type, null, OptionConfigurationPropertyBehavior.None, null)
		{
			if(string.IsNullOrWhiteSpace(elementName))
				throw new ArgumentNullException(nameof(elementName));

			_elementName = elementName.Trim();
		}

		public OptionConfigurationProperty(string name, Type type, object defaultValue) : this(name, type, defaultValue, OptionConfigurationPropertyBehavior.None, null)
		{

		}

		public OptionConfigurationProperty(string name, Type type, object defaultValue, OptionConfigurationPropertyBehavior behavior) : this(name, type, defaultValue, behavior, null)
		{

		}

		public OptionConfigurationProperty(string name, Type type, object defaultValue, OptionConfigurationPropertyBehavior behavior, TypeConverter converter)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			_name = name == null ? string.Empty : name.Trim();
			_type = type;
			_behavior = behavior;
			_converter = converter;

			//注意：要最后设置默认属性的值
			this.DefaultValue = defaultValue;
		}

		internal OptionConfigurationProperty(PropertyInfo propertyInfo)
		{
			OptionConfigurationPropertyAttribute propertyAttribute = null;
			TypeConverterAttribute converterAttribute = null;
			DefaultValueAttribute defaultAttribute = null;

			var attributes = propertyInfo.GetCustomAttributes();

			foreach(var attribute in attributes)
			{
				if(attribute is OptionConfigurationPropertyAttribute)
					propertyAttribute = (OptionConfigurationPropertyAttribute)attribute;
				else if(attribute is DefaultValueAttribute)
					defaultAttribute = (DefaultValueAttribute)attribute;
				else if(attribute is TypeConverterAttribute)
					converterAttribute = (TypeConverterAttribute)attribute;
			}

			_name = propertyAttribute.Name;
			_elementName = propertyAttribute.ElementName;
			_type = propertyAttribute.Type ?? propertyInfo.PropertyType;
			_behavior = propertyAttribute.Behavior;

			if(propertyAttribute.Converter != null)
				_converter = propertyAttribute.Converter;
			else
			{
				if(converterAttribute != null && !string.IsNullOrEmpty(converterAttribute.ConverterTypeName))
				{
					Type type = Type.GetType(converterAttribute.ConverterTypeName, false);

					if(type != null)
						_converter = Activator.CreateInstance(type, true) as TypeConverter;
				}
			}

			//注意：要最后设置默认属性的值
			this.DefaultValue = defaultAttribute != null ? defaultAttribute.Value : propertyAttribute.DefaultValue;
		}

		#endregion
	}
}