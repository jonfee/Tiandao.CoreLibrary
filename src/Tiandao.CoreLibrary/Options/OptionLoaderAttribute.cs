using System;
using System.Collections.Generic;

namespace Tiandao.Options
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class OptionLoaderAttribute : Attribute
	{
		#region 私有字段

		private Type _loaderType;

		#endregion

		#region 公共属性

		public Type LoaderType
		{
			get
			{
				return _loaderType;
			}
			set
			{
				_loaderType = value;
			}
		}

		#endregion
	}
}