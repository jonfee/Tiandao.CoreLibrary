using System;
using System.Collections.Generic;

namespace Tiandao.Options.Configuration
{
    public class OptionConfigurationLoader : IOptionLoader
	{
		#region 私有字段

		private OptionNode _root;

		#endregion

		#region 公共属性

		public OptionNode RootNode
		{
			get
			{
				return _root;
			}
		}

		#endregion

		#region 构造方法

		public OptionConfigurationLoader(OptionNode rootNode)
		{
			if(rootNode == null)
				throw new ArgumentNullException(nameof(rootNode));

			_root = rootNode;
		}

		#endregion

		#region 公共方法

		public virtual void Load(IOptionProvider provider)
		{
			this.LoadConfiguration(provider as OptionConfiguration);
		}

		public virtual void Unload(IOptionProvider provider)
		{
			this.UnloadConfiguration(provider as OptionConfiguration);
		}

		#endregion

		#region 保护方法
		public void LoadConfiguration(OptionConfiguration configuration)
		{
			if(configuration == null)
				return;

			foreach(var section in configuration.Sections)
			{
				//必须先确保选项节对应的空节点被添加
				var sectionNode = _root.FindNode(section.Path, token =>
				{
					if(token.Current == null)
					{
						var parent = token.Parent as OptionNode;

						if(parent != null)
							return parent.Children.Add(token.Name);
					}

					return token.Current;
				});

				//在添加了选项上级空节点添加完成之后再添加选项元素的节点
				foreach(var elementName in section.Children.Keys)
				{
					var node = (OptionNode)_root.FindNode(new string[] { section.Path, elementName }, token =>
					{
						if(token.Current == null)
						{
							var parent = token.Parent as OptionNode;

							if(parent != null)
								return parent.Children.Add(token.Name, configuration);
						}

						return token.Current;
					});
				}
			}
		}

		public void UnloadConfiguration(OptionConfiguration configuration)
		{
			if(configuration == null)
				return;

			foreach(var section in configuration.Sections)
			{
				foreach(var elementName in section.Children.Keys)
				{
					var node = _root.Find(section.Path, elementName);

					if(node != null)
					{
						var option = node.Option;

						if(option != null && option.Provider == configuration)
						{
							node.Option = null;

							var parent = node.Parent;
							if(parent != null)
								parent.Children.Remove(node);
						}
					}
				}
			}
		}

		#endregion
	}
}