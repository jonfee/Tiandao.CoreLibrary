using System;
using System.Collections.Generic;

using Tiandao.Options.Configuration;

namespace Tiandao.Diagnostics.Configuration
{
    public class LoggerElement : OptionConfigurationElement
    {
		#region 公共属性

		[OptionConfigurationProperty("")]
		public LoggerHandlerElementCollection Handlers
		{
			get
			{
				return (LoggerHandlerElementCollection)this[""];
			}
		}

		#endregion
	}
}
