using System;
using System.Collections.Generic;

namespace Tiandao.Options.Configuration
{
    public class SettingElementCollection : OptionConfigurationElementCollection<SettingElement>, ISettingsProvider
	{
		#region 公共属性

		/// <summary>
		/// 获取或设置指定设置项的文本值。
		/// </summary>
		/// <param name="name">指定要获取或设置的项目名称。</param>
		/// <returns>返回指定名称对应的文本值，如果指定的名称不存在则返回空(null)，如果属性设置器中(setter)中<paramref name="value"/>参数为空(null)，则表示将其指定名称的设置项删除。</returns>
		public new string this[string name]
		{
			get
			{
				var element = base.Find(name) as SettingElement;

				if(element != null)
					return element.Value;
				else
					return null;
			}
			set
			{
				if(string.IsNullOrWhiteSpace(name))
					throw new ArgumentNullException(nameof(name));

				if(value == null)
					this.Remove(name);
				else
				{
					var element = base.Find(name) as SettingElement;

					if(element != null)
						element.Value = value;
					else
						this.Add(new SettingElement(name, value));
				}
			}
		}

		#endregion

		#region 构造方法

		public SettingElementCollection() : base("setting")
		{

		}

		protected SettingElementCollection(string elementName) : base(elementName)
		{

		}

		#endregion

		#region 重写方法

		protected override OptionConfigurationElement CreateNewElement()
		{
			return new SettingElement();
		}

		protected override string GetElementKey(OptionConfigurationElement element)
		{
			return ((SettingElement)element).Name;
		}

		#endregion

		#region 显式实现

		object ISettingsProvider.GetValue(string name)
		{
			return this[name];
		}

		void ISettingsProvider.SetValue(string name, object value)
		{
			if(value == null)
				this[name] = null;
			else
				this[name] = value.ToString();
		}

		#endregion
	}
}