using System;
using System.ComponentModel;

namespace Tiandao.Collections
{
	#if !CORE_CLR
	[Serializable]
	#endif
	public class CategoryBase : HierarchicalNode
	{
		#region 私有字段

		private string _title;
		private string _description;
		private string _tags;
		private bool _visible;

		#endregion

		#region 公共属性

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
			}
		}

		public string Tags
		{
			get
			{
				return _tags;
			}
			set
			{
				_tags = value;
			}
		}

		[DefaultValue(true)]
		public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				_visible = value;
			}
		}

		#endregion

		#region 构造方法

		protected CategoryBase()
		{

		}

		protected CategoryBase(string name) : this(name, name, string.Empty, true)
		{

		}

		protected CategoryBase(string name, string title, string description) : this(name, title, description, true)
		{

		}

		protected CategoryBase(string name, string title, string description, bool visible) : base(name)
		{
			_title = string.IsNullOrEmpty(title) ? name : title;
			_description = description;
			_visible = visible;
		}

		#endregion
	}
}
