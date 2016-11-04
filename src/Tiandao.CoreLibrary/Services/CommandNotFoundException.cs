using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public class CommandNotFoundException : CommandException
	{
		#region 私有字段

		private string _path;

		#endregion

		#region 公共属性

		public string Path
		{
			get
			{
				return _path;
			}
		}

		#endregion

		#region 构造方法

		public CommandNotFoundException(string path)
		{
			_path = path ?? string.Empty;
		}

		#endregion
	}
}
