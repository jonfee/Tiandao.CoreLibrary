using System;
using System.ComponentModel;

namespace Tiandao.Options.Configuration
{
	[AttributeUsage(AttributeTargets.Property)]
	public class OptionConfigurationPropertyAttribute : Attribute
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
			set
			{
				if(string.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException();

				_elementName = value.Trim();
			}
		}

		public Type Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
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
				_defaultValue = value;
			}
		}

		public OptionConfigurationPropertyBehavior Behavior
		{
			get
			{
				return _behavior;
			}
			set
			{
				_behavior = value;
			}
		}

		public TypeConverter Converter
		{
			get
			{
				return _converter;
			}
			set
			{
				_converter = value;
			}
		}

		#endregion

		#region 构造方法

		public OptionConfigurationPropertyAttribute(string name) : this(name, null, null, OptionConfigurationPropertyBehavior.None, null)
		{

		}

		public OptionConfigurationPropertyAttribute(string name, OptionConfigurationPropertyBehavior behavior) : this(name, null, null, behavior, null)
		{

		}

		public OptionConfigurationPropertyAttribute(string name, object defaultValue) : this(name, null, defaultValue, OptionConfigurationPropertyBehavior.None, null)
		{

		}

		public OptionConfigurationPropertyAttribute(string name, Type type, object defaultValue, OptionConfigurationPropertyBehavior behavior, TypeConverter converter)
		{
			_name = name == null ? string.Empty : name.Trim();
			_type = type;
			_defaultValue = defaultValue;
			_behavior = behavior;
			_converter = converter;
		}

		#endregion
	}
}