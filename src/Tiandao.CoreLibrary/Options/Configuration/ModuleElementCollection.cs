using System;
using System.Collections.Generic;

namespace Tiandao.Options.Configuration
{
    public class ModuleElementCollection : OptionConfigurationElementCollection
	{
		#region 公共属性

		public ModuleElement this[int index]
		{
			get
			{
				return (ModuleElement)this.Items[index];
			}
		}

		public new ModuleElement this[string name]
		{
			get
			{
				return base.Find(name) as ModuleElement;
			}
		}

		#endregion

		#region 构造函数

		public ModuleElementCollection() : base("module")
		{

		}

		protected ModuleElementCollection(string elementName) : base(elementName)
		{

		}

		#endregion

		#region 重写方法

		protected override OptionConfigurationElement CreateNewElement()
		{
			return new ModuleElement();
		}

		protected override string GetElementKey(OptionConfigurationElement element)
		{
			return ((ModuleElement)element).Name;
		}

		#endregion
	}
}
