using System;

namespace Tiandao.ComponentModel
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public class AliasAttribute : Attribute
	{
		#region 私有字段

		private string _alias;

		#endregion

		#region 公共属性

		public string Alias
		{
			get
			{
				return _alias;
			}
		}

		#endregion

		#region 构造方法

		public AliasAttribute(string alias)
		{
			_alias = alias ?? string.Empty;
		}

		#endregion
	}
}