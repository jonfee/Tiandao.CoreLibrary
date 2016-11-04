using System;
using System.Linq;

namespace Tiandao.Options.Configuration
{
    internal static class OptionConfigurationUtility
    {
		public static OptionConfigurationElement GetGlobalElement(string elementName)
		{
			if(!OptionConfiguration.Declarations.Contains(elementName))
				return null;

			var declaration = OptionConfiguration.Declarations[elementName];
			return Activator.CreateInstance(declaration.Type) as OptionConfigurationElement;
		}

		public static OptionConfigurationProperty GetKeyProperty(OptionConfigurationElement element)
		{
			if(element == null)
				return null;

			return element.Properties.FirstOrDefault(property => property.IsKey);
		}

		public static OptionConfigurationProperty GetDefaultCollectionProperty(OptionConfigurationPropertyCollection properties)
		{
			if(properties == null || properties.Count < 1)
				return null;

			return properties.FirstOrDefault(property => property.IsDefaultCollection);
		}

		public static string GetValueString(object value, System.ComponentModel.TypeConverter converter)
		{
			if(value == null)
				return string.Empty;

			if(value is string)
				return (string)value;

			if(converter != null)
				return converter.ConvertToString(value);
			else
				return Common.Converter.ConvertValue<string>(value);
		}
	}
}