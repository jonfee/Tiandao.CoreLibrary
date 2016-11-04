using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
#if !CORE_CLR
	[Serializable]
#endif
	public class CommandOption
    {
		#region 私有字段

		private string _name;
		private object _value;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取命令选项的名称。
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
		}

		/// <summary>
		/// 获取或设置命令选项的值。
		/// </summary>
		public object Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		#endregion

		#region 构造方法

		public CommandOption(string name) : this(name, null)
		{
		}

		public CommandOption(string name, object value)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));

			_name = name.Trim();
			_value = value;
		}

		#endregion
	}
}
