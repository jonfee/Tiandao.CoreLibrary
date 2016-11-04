using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tiandao.Options.Configuration
{
    public class ModuleElement : OptionConfigurationElement
	{
		#region 常量定义

		private const string XML_NAME_ATTRIBUTE = "name";
		private const string XML_TYPE_ATTRIBUTE = "type";

		#endregion

		#region 公共属性

		[OptionConfigurationProperty(XML_NAME_ATTRIBUTE)]
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

				this[XML_NAME_ATTRIBUTE] = value.Trim();
			}
		}

		[OptionConfigurationProperty(XML_TYPE_ATTRIBUTE)]
		public string Type
		{
			get
			{
				return (string)this[XML_TYPE_ATTRIBUTE];
			}
			set
			{
				this[XML_TYPE_ATTRIBUTE] = value;
			}
		}

		#endregion

		#region 构造方法

		internal ModuleElement()
		{

		}

		internal ModuleElement(string name, string type)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));

			if(string.IsNullOrWhiteSpace(type))
				throw new ArgumentNullException(nameof(type));

			this.Name = name;
			this.Type = type;
		}

		#endregion

		#region 公共方法

		public ComponentModel.IApplicationModule CreateModule()
		{
			var typeName = this.Type;

			if(string.IsNullOrWhiteSpace(typeName))
				throw new OptionConfigurationException("The module type is empty or unspecified.");

			var type = System.Type.GetType(typeName, false);

			if(type == null)
				throw new OptionConfigurationException(string.Format("Invalid '{0}' type of module, becase cann't load it.", typeName));

			if(!typeof(ComponentModel.IApplicationModule).IsAssignableFrom(type))
				throw new OptionConfigurationException(string.Format("Invalid '{0}' type of module, it doesn't implemented IModule interface.", typeName));

			return Activator.CreateInstance(type) as ComponentModel.IApplicationModule;
		}

		#endregion
	}
}
