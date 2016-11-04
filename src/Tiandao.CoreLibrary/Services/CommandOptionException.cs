using System;
using System.Collections.Generic;
using Tiandao.Common;

namespace Tiandao.Services
{
#if !CORE_CLR
	[Serializable]
#endif
	public class CommandOptionException : CommandException
    {
		#region 私有字段

		private string _optionName;

		#endregion

		#region 公共属性

		public string OptionName
		{
			get
			{
				return _optionName;
			}
		}

		#endregion

		#region 构造方法

		public CommandOptionException(string optionName) : this(optionName, Resources.ResourceUtility.GetString("InvalidCommandOption", typeof(CommandOptionException).GetAssembly(), optionName))
		{

		}

		public CommandOptionException(string optionName, string message) : base(message)
		{
			if(string.IsNullOrWhiteSpace(optionName))
				throw new ArgumentNullException(nameof(optionName));

			_optionName = optionName.Trim();
		}

		#endregion
	}
}
