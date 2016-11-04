using System;
using System.Collections.Generic;

namespace Tiandao.Options
{
    public class OptionNode : Collections.HierarchicalNode
    {
		#region 私有字段

		private IOption _option;
		private OptionNodeCollection _children;
		private string _title;
		private string _description;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取或设置选项节点的标题文本。
		/// </summary>
		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value ?? string.Empty;
			}
		}

		/// <summary>
		/// 获取或设置选项节点的描述文本。
		/// </summary>
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value ?? string.Empty;
			}
		}

		/// <summary>
		/// 获取或设置选项节点对应的选项对象。
		/// </summary>
		public IOption Option
		{
			get
			{
				return _option;
			}
			set
			{
				if(object.ReferenceEquals(_option, value))
					return;

				_option = value;
			}
		}

		/// <summary>
		/// 获取选项节点的父节点，根节点的父节点为空(null)。
		/// </summary>
		public OptionNode Parent
		{
			get
			{
				return (OptionNode)base.InnerParent;
			}
			internal set
			{
				base.InnerParent = value;
			}
		}

		/// <summary>
		/// 获取选项节点的子节点集合。
		/// </summary>
		public OptionNodeCollection Children
		{
			get
			{
				//确认子节点集合是否已经加载过
				this.EnsureChildren();

				return _children;
			}
		}

		#endregion

		#region 构造方法

		internal OptionNode()
		{
			_children = new OptionNodeCollection(this);
		}

		public OptionNode(string name) : this(name, name, null)
		{
		}

		public OptionNode(string name, string title, string description) : base(name)
		{
			_title = string.IsNullOrWhiteSpace(title) ? name : title;
			_description = description ?? string.Empty;
			_children = new OptionNodeCollection(this);
		}

		public OptionNode(string name, IOption option) : base(name)
		{
			if(option == null)
				throw new ArgumentNullException(nameof(option));

			_option = option;
			_children = new OptionNodeCollection(this);
		}

		#endregion

		#region 公共方法

		public OptionNode Find(string path)
		{
			return base.FindNode(path) as OptionNode;
		}

		public OptionNode Find(params string[] parts)
		{
			return base.FindNode(parts) as OptionNode;
		}

		#endregion

		#region 重写方法

		protected override Collections.HierarchicalNode GetChild(string name)
		{
			return _children[name];
		}

		#endregion
	}
}