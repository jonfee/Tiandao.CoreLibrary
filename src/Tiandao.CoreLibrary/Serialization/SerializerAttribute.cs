using System;
using System.Collections.Generic;

namespace Tiandao.Serialization
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter)]
	public class SerializerAttribute : Attribute
	{
		#region 成员字段

		private Type _type;

		#endregion

		#region 公共属性

		public Type Type
		{
			get
			{
				return _type;
			}
		}

		#endregion

		#region 构造方法

		public SerializerAttribute(Type type)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			_type = type;
		}

		#endregion
	}
}
