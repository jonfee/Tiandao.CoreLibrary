using System;
using System.Collections.Generic;

namespace Tiandao.Options.Profiles
{
#if !CORE_CLR
	[Serializable]
#endif
	public class ProfileEntry : ProfileItem
	{
		#region 私有字段

		private string _name;
		private string _value;

		#endregion

		#region 公共属性

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public string Value
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

		public ProfileSection Section
		{
			get
			{
				return (ProfileSection)base.Owner;
			}
		}

		public override ProfileItemType ItemType
		{
			get
			{
				return ProfileItemType.Entry;
			}
		}

		#endregion

		#region 构造方法

		public ProfileEntry(string name, string value = null) : this(-1, name, value)
		{
		}

		public ProfileEntry(int lineNumber, string name, string value = null) : base(lineNumber)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			_name = name.Trim();

			if(value != null)
				_value = value.Trim();
		}

		#endregion

		#region 重写方法

		public override string ToString()
		{
			if(_value == null)
				return _name;

			return string.Format("{0}={1}", _name, _value);
		}

		#endregion
	}
}
