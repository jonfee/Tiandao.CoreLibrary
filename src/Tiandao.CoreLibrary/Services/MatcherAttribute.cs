using System;
using System.Reflection;
using System.Collections.Generic;

namespace Tiandao.Services
{
	[AttributeUsage(AttributeTargets.Class)]
	public class MatcherAttribute : Attribute
	{
		#region 私有字段

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

		public IMatcher Matcher
		{
			get
			{
				if(_type == null)
					return null;

				return Activator.CreateInstance(_type) as IMatcher;
			}
		}

		#endregion

		#region 构造方法

		public MatcherAttribute(Type type)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			if(!typeof(IMatcher).IsAssignableFrom(type))
				throw new ArgumentException("The type is not a IMatcher.");

			_type = type;
		}

		public MatcherAttribute(string typeName)
		{
			if(string.IsNullOrWhiteSpace(typeName))
				throw new ArgumentNullException(nameof(typeName));

			var type = Type.GetType(typeName, false);

			if(type == null || !typeof(IMatcher).IsAssignableFrom(type))
				throw new ArgumentException("The type is not a IMatcher.");

			_type = type;
		}

		#endregion

	}
}