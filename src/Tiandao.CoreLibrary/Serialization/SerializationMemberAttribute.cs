using System;
using System.Collections.Generic;

namespace Tiandao.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
	public class SerializationMemberAttribute : Attribute
	{
		#region 私有字段

		private string _name;
		private SerializationMemberBehavior _behavior;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取或设置序列化后的成员名称，如果为空(null)或空字符串("")则取对应的成员本身的名称。
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value == null ? string.Empty : value.Trim();
			}
		}

		/// <summary>
		/// 获取或设置成员的序列化行为。
		/// </summary>
		public SerializationMemberBehavior Behavior
		{
			get
			{
				return _behavior;
			}
			set
			{
				_behavior = value;
			}
		}

		#endregion

		#region 构造方法

		public SerializationMemberAttribute()
		{
		}

		public SerializationMemberAttribute(string name)
		{
			_name = name == null ? string.Empty : name.Trim();
		}

		public SerializationMemberAttribute(SerializationMemberBehavior behavior)
		{
			_behavior = behavior;
		}

		#endregion
	}
}
