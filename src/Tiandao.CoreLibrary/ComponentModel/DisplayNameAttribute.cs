using System;
using System.Collections.Generic;

namespace Tiandao.ComponentModel
{
	[AttributeUsage(AttributeTargets.Event | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class)]
	public class DisplayNameAttribute : Attribute
	{
		#region 私有字段

		private string _displayName;

		#endregion

		#region 静态字段

		public static readonly DisplayNameAttribute Default = new DisplayNameAttribute();

		#endregion

		#region 公共属性

		public virtual string DisplayName
		{
			get
			{
				return this.DisplayNameValue;
			}
		}

		protected string DisplayNameValue
		{
			get
			{
				return this._displayName;
			}
			set
			{
				this._displayName = value;
			}
		}

		#endregion

		#region 构造方法

		public DisplayNameAttribute() : this(string.Empty)
		{
		}

		public DisplayNameAttribute(string displayName)
		{
			this._displayName = displayName;
		}

		#endregion

		#region 重写方法

		public override bool Equals(object obj)
		{
			if(obj == this)
			{
				return true;
			}
			DisplayNameAttribute attribute = obj as DisplayNameAttribute;
			return ((attribute != null) && (attribute.DisplayName == this.DisplayName));
		}

		public override int GetHashCode()
		{
			return this.DisplayName.GetHashCode();
		}

#if !CORE_CLR
		public override bool IsDefaultAttribute()
		{
			return this.Equals(Default);
		}
#endif

		#endregion
	}
}