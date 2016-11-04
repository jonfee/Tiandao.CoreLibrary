using System;
using System.Collections.Generic;

namespace Tiandao.Options
{
    public class OptionNodeCollection : Collections.HierarchicalNodeCollection<OptionNode>
    {
		#region 构造方法

		internal OptionNodeCollection(OptionNode owner) : base(owner)
		{

		}

		#endregion

		#region 公共方法

		public OptionNode Add(string name, string title = null, string description = null)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));

			var node = new OptionNode(name, title, description);
			this.Add(node);
			return node;
		}

		public OptionNode Add(string name, IOptionProvider provider, string title = null, string description = null)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));

			OptionNode node = new OptionNode(name, title, description);

			if(provider != null)
				node.Option = new Option(node, provider);

			this.Add(node);
			return node;
		}

		#endregion
	}
}