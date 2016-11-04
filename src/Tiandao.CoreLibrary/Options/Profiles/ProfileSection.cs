using System;
using System.Collections.Generic;

namespace Tiandao.Options.Profiles
{
    public class ProfileSection : ProfileItem, ISettingsProvider
	{
		#region 私有字段

		private string _name;
		private ProfileItemCollection _items;
		private ProfileEntryCollection _entries;
		private ProfileCommentCollection _comments;
		private ProfileSectionCollection _sections;

		#endregion

		#region 公共属性

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public string FullName
		{
			get
			{
				var parent = this.Parent;

				if(parent == null)
					return this.Name;
				else
					return parent.FullName + " " + this.Name;
			}
		}

		public ProfileSection Parent
		{
			get
			{
				return base.Owner as ProfileSection;
			}
		}

		public ProfileItemCollection Items
		{
			get
			{
				return _items;
			}
		}

		public ProfileEntryCollection Entries
		{
			get
			{
				if(_entries == null)
					System.Threading.Interlocked.CompareExchange(ref _entries, new ProfileEntryCollection(_items), null);

				return _entries;
			}
		}

		public ProfileCommentCollection Comments
		{
			get
			{
				if(_comments == null)
					System.Threading.Interlocked.CompareExchange(ref _comments, new ProfileCommentCollection(_items), null);

				return _comments;
			}
		}

		public ProfileSectionCollection Sections
		{
			get
			{
				if(_sections == null)
					System.Threading.Interlocked.CompareExchange(ref _sections, new ProfileSectionCollection(_items), null);

				return _sections;
			}
		}

		public override ProfileItemType ItemType
		{
			get
			{
				return ProfileItemType.Section;
			}
		}

		#endregion

		#region 构造方法

		public ProfileSection(string name, int lineNumber = -1) : base(lineNumber)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			if(Common.StringExtension.ContainsCharacters(name, "/*?"))
				throw new ArgumentException();

			_name = name.Trim();
			_items = new ProfileItemCollection(this);
		}

		#endregion

		#region 重写方法

		protected override void OnOwnerChanged(object owner)
		{
			_items.Owner = owner;
		}

		#endregion

		#region 公共方法

		public string GetEntryValue(string name)
		{
			var entry = this.Entries[name];

			if(entry != null)
				return entry.Value;

			return null;
		}

		public void SetEntryValue(string name, string value)
		{
			var entry = this.Entries[name];

			if(entry != null)
				entry.Value = value;
			else
				this.Entries.Add(name, value);
		}

		#endregion

		#region 接口实现

		object ISettingsProvider.GetValue(string name)
		{
			return this.GetEntryValue(name);
		}

		void ISettingsProvider.SetValue(string name, object value)
		{
			this.SetEntryValue(name, Common.Converter.ConvertValue<string>(value));
		}

		#endregion

	}
}
