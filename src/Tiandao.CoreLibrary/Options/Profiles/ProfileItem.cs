using System;
using System.Collections.Generic;

namespace Tiandao.Options.Profiles
{
    public abstract class ProfileItem
    {
		#region 私有字段

		private object _owner;
		private int _lineNumber;

		#endregion

		#region 公共属性

		public virtual Profile Profile
		{
			get
			{
				return _owner as Profile;
			}
		}

		public abstract ProfileItemType ItemType
		{
			get;
		}

		public int LineNumber
		{
			get
			{
				return _lineNumber;
			}
		}

		#endregion

		#region 保护属性

		internal protected object Owner
		{
			get
			{
				return _owner;
			}
			internal set
			{
				if(value == null)
					throw new ArgumentNullException();

				if(object.ReferenceEquals(_owner, value))
					return;

				_owner = value;
				this.OnOwnerChanged(value);
			}
		}

		#endregion

		#region 构造方法

		protected ProfileItem()
		{
			_lineNumber = -1;
		}

		protected ProfileItem(int lineNumber)
		{
			_lineNumber = Math.Max(lineNumber, -1);
		}

		protected ProfileItem(Profile owner, int lineNumber)
		{
			if(owner == null)
				throw new ArgumentNullException("owner");

			_owner = owner;
			_lineNumber = Math.Max(lineNumber, -1);
		}

		protected ProfileItem(ProfileSection owner, int lineNumber)
		{
			if(owner == null)
				throw new ArgumentNullException("owner");

			_owner = owner;
			_lineNumber = Math.Max(lineNumber, -1);
		}

		#endregion

		#region 虚拟方法

		protected virtual void OnOwnerChanged(object owner)
		{

		}

		#endregion
	}
}
