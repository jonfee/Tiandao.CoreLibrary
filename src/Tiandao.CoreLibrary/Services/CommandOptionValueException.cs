using System;
using System.Collections.Generic;
using Tiandao.Common;

namespace Tiandao.Services
{
#if !CORE_CLR
	[Serializable]
#endif
	public class CommandOptionValueException : CommandOptionException
    {
		#region 私有字段

		private object _optionValue;

		#endregion

		#region 公共属性

		public object OptionValue
		{
			get
			{
				return _optionValue;
			}
		}

		#endregion

		#region 构造方法

		public CommandOptionValueException(string optionName, object optionValue) : base(optionName, Resources.ResourceUtility.GetString("InvalidCommandOptionValue", typeof(CommandOptionValueException).GetAssembly(), optionName, optionValue))
		{
			_optionValue = optionValue;
		}

		#endregion
	}
}