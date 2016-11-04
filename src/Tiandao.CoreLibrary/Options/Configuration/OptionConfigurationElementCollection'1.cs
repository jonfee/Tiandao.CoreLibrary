using System;
using System.Collections.Generic;

namespace Tiandao.Options.Configuration
{
    public class OptionConfigurationElementCollection<T> : OptionConfigurationElementCollection where T : OptionConfigurationElement
	{
		#region 公共属性

		public T this[int index]
		{
			get
			{
				return (T)base.Items[index];
			}
		}

		public new T this[string key]
		{
			get
			{
				return (T)base.Find(key);
			}
		}

		#endregion

		#region 构造方法

		public OptionConfigurationElementCollection(string elementName, IEqualityComparer<string> comparer = null) : base(elementName, comparer)
		{

		}

		protected OptionConfigurationElementCollection()
		{

		}

		#endregion

		#region 重写方法

		protected override OptionConfigurationElement CreateNewElement()
		{
			return Activator.CreateInstance<T>();
		}

		protected override string GetElementKey(OptionConfigurationElement element)
		{
			var property = OptionConfigurationUtility.GetKeyProperty(element);
			if(property == null)
				throw new OptionConfigurationException();

			var value = element[property];
			if(value == null)
				throw new OptionConfigurationException();

			return (string)value.ToString();
		}

		#endregion
	}
}