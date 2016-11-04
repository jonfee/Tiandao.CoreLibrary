using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public class CommandTreeNodeCollection : Collections.HierarchicalNodeCollection<CommandTreeNode>
	{
		#region 构造方法

		public CommandTreeNodeCollection(CommandTreeNode owner) : base(owner)
		{

		}

		#endregion

		#region 公共方法

		public CommandTreeNode Add(string name)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			var node = new CommandTreeNode(name, this.Owner);
			this.Add(node);
			return node;
		}

		public CommandTreeNode Add(ICommand command)
		{
			if(command == null)
				throw new ArgumentNullException("command");

			var node = new CommandTreeNode(command, this.Owner);
			this.Add(node);
			return node;
		}

		#endregion

		#region 重写方法
		protected override bool TryConvertItem(object value, out CommandTreeNode item)
		{
			if(value is ICommand)
			{
				item = new CommandTreeNode((ICommand)value, this.Owner);
				return true;
			}

			return base.TryConvertItem(value, out item);
		}

		#endregion
	}
}