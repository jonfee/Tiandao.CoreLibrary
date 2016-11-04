using System;
using System.Collections.Generic;

namespace Tiandao.Options.Configuration
{
    public class OptionConfigurationSection
    {
		#region 私有字段

		private string _path;
		private IDictionary<string, OptionConfigurationElement> _children;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取当前选项申明节的逻辑路径，即选项路径。
		/// </summary>
		public string Path
		{
			get
			{
				return _path;
			}
		}

		public OptionConfigurationElement this[string name]
		{
			get
			{
				if(string.IsNullOrWhiteSpace(name))
					throw new ArgumentNullException(nameof(name));

				return _children[name];
			}
		}

		public IDictionary<string, OptionConfigurationElement> Children
		{
			get
			{
				return _children;
			}
		}

		#endregion

		#region 构造方法

		public OptionConfigurationSection(string path)
		{
			if(string.IsNullOrWhiteSpace(path))
				throw new ArgumentNullException(nameof(path));

			_path = path.Trim().Trim('/');
			_children = new Dictionary<string, OptionConfigurationElement>(StringComparer.OrdinalIgnoreCase);
		}

		#endregion
	}
}