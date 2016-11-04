using System;
using System.Collections.Generic;

using Tiandao.Options.Configuration;

namespace Tiandao.Diagnostics.Configuration
{
    public class LoggerHandlerElementCollection : OptionConfigurationElementCollection<LoggerHandlerElement>
	{
		#region 构造方法

		public LoggerHandlerElementCollection() : base("handler")
		{

		}

		#endregion

		#region 重写方法

		protected override OptionConfigurationElement CreateNewElement()
		{
			return new LoggerHandlerElement();
		}

		protected override string GetElementKey(OptionConfigurationElement element)
		{
			return ((LoggerHandlerElement)element).Name;
		}

		#endregion
	}
}
