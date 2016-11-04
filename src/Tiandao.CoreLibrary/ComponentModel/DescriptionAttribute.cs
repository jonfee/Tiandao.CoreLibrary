using System;
using System.Text;

namespace Tiandao.ComponentModel
{
	/// <summary>
	/// 指定属性 (Property) 或事件的说明。
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class DescriptionAttribute : Attribute
	{
		#region 静态常量

		public static readonly DescriptionAttribute Default = new DescriptionAttribute();

		#endregion

		#region 私有字段

		private string _description;

		#endregion

		#region 公共属性

		public virtual string Description
		{
			get
			{
				return this.DescriptionValue;
			}
		}

		#endregion

		#region 保护属性

		protected string DescriptionValue
		{
			get
			{
				return this._description;
			}
			set
			{
				this._description = value;
			}
		}

		#endregion

		#region 构造方法

		public DescriptionAttribute() : this(string.Empty)
		{
		}

		public DescriptionAttribute(string description)
		{
			this._description = description;
		}

		#endregion

		#region 重写方法

		public override bool Equals(object obj)
		{
			if(obj == this)
				return true;

			DescriptionAttribute attribute = obj as DescriptionAttribute;

			return ((attribute != null) && (attribute.Description == this.Description));
		}

		public override int GetHashCode()
		{
			return this.Description.GetHashCode();
		}

		#endregion
	}
}
