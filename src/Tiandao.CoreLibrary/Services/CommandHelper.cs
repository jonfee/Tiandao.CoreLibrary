using System;
using System.Collections.Generic;
using System.Linq;

using Tiandao.Common;

namespace Tiandao.Services
{
    internal static class CommandHelper
    {
		private static readonly Dictionary<ICommand, CommandOptionAttribute[]> _options = new Dictionary<ICommand, CommandOptionAttribute[]>();

		internal static CommandOptionAttribute[] GetOptions(ICommand command)
		{
			if(command == null)
				throw new ArgumentNullException(nameof(command));

			CommandOptionAttribute[] result;

			if(_options.TryGetValue(command, out result))
				return result;

			lock (((System.Collections.ICollection)_options).SyncRoot)
			{
				var attributes = (CommandOptionAttribute[])command.GetType().GetCustomAttributes(typeof(CommandOptionAttribute), true);

				_options[command] = attributes;
				return attributes;
			}
		}

		internal static CommandOptionAttribute GetOption(ICommand command, string name)
		{
			var attributes = GetOptions(command);

			return attributes.FirstOrDefault(att => string.Equals(att.Name, name, StringComparison.OrdinalIgnoreCase));
		}
	}
}
