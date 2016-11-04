using System;
using System.Collections.Generic;

using Tiandao.Options.Configuration;

namespace Tiandao.Diagnostics.Configuration
{
	public class LoggerHandlerElement : OptionConfigurationElement
	{
		#region 常量定义

		private const string XML_NAME_ATTRIBUTE = "name";
		private const string XML_TYPE_ATTRIBUTE = "type";
		private const string XML_PREDICATION_ELEMENT = "predication";
		private const string XML_PARAMETERS_COLLECTION = "parameters";

		#endregion

		#region 公共属性

		[OptionConfigurationProperty(XML_NAME_ATTRIBUTE, Behavior = OptionConfigurationPropertyBehavior.IsKey)]
		public string Name
		{
			get
			{
				return (string)this[XML_NAME_ATTRIBUTE];
			}
			set
			{
				if(string.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException();

				this[XML_NAME_ATTRIBUTE] = value;
			}
		}

		[OptionConfigurationProperty(XML_TYPE_ATTRIBUTE, Behavior = OptionConfigurationPropertyBehavior.IsRequired)]
		public string TypeName
		{
			get
			{
				return (string)this[XML_TYPE_ATTRIBUTE];
			}
			set
			{
				if(string.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException();

				this[XML_TYPE_ATTRIBUTE] = value;
			}
		}

		[OptionConfigurationProperty(XML_PREDICATION_ELEMENT)]
		public LoggerHandlerPredicationElement Predication
		{
			get
			{
				return (LoggerHandlerPredicationElement)this[XML_PREDICATION_ELEMENT];
			}
		}

		[OptionConfigurationProperty(XML_PARAMETERS_COLLECTION, ElementName = "parameter")]
		public SettingElementCollection Parameters
		{
			get
			{
				return (SettingElementCollection)this[XML_PARAMETERS_COLLECTION];
			}
		}

		public bool HasExtendedProperties
		{
			get
			{
				return base.HasUnrecognizedProperties;
			}
		}

		public IDictionary<string, string> ExtendedProperties
		{
			get
			{
				return base.UnrecognizedProperties;
			}
		}

		#endregion

		#region 重写方法

		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			this.UnrecognizedProperties.Add(name, value);

			return true;
		}

		#endregion

		#region 嵌套子类

		public class LoggerHandlerPredicationElement : OptionConfigurationElement
		{
			#region 常量定义

			private const string XML_SOURCE_ATTRIBUTE = "source";
			private const string XML_EXCEPTIONTYPE_ATTRIBUTE = "exceptionType";
			private const string XML_MINLEVEL_ATTRIBUTE = "minLevel";
			private const string XML_MAXLEVEL_ATTRIBUTE = "maxLevel";

			#endregion

			#region 公共属性

			[OptionConfigurationProperty(XML_SOURCE_ATTRIBUTE)]
			public string Source
			{
				get
				{
					return (string)this[XML_SOURCE_ATTRIBUTE];
				}
				set
				{
					this[XML_SOURCE_ATTRIBUTE] = value;
				}
			}

			[OptionConfigurationProperty(XML_EXCEPTIONTYPE_ATTRIBUTE)]
			public Type ExceptionType
			{
				get
				{
					return (Type)this[XML_EXCEPTIONTYPE_ATTRIBUTE];
				}
				set
				{
					this[XML_EXCEPTIONTYPE_ATTRIBUTE] = value;
				}
			}

			[OptionConfigurationProperty(XML_MINLEVEL_ATTRIBUTE)]
			public LogLevel? MinLevel
			{
				get
				{
					return (LogLevel?)this[XML_MINLEVEL_ATTRIBUTE];
				}
				set
				{
					this[XML_MINLEVEL_ATTRIBUTE] = value;
				}
			}

			[OptionConfigurationProperty(XML_MAXLEVEL_ATTRIBUTE)]
			public LogLevel? MaxLevel
			{
				get
				{
					return (LogLevel?)this[XML_MAXLEVEL_ATTRIBUTE];
				}
				set
				{
					this[XML_MAXLEVEL_ATTRIBUTE] = value;
				}
			}

			#endregion
		}

		#endregion
	}
}