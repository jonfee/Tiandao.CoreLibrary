using System;
using System.Collections.Generic;
using Tiandao.Common;

namespace Tiandao.Services
{
    public class CommandOptionMissingException : CommandOptionException
	{
		public CommandOptionMissingException(string optionName) : base(optionName, Resources.ResourceUtility.GetString("InvalidCommandOption", typeof(CommandOptionMissingException).GetAssembly(), optionName))
		{

		}
	}
}