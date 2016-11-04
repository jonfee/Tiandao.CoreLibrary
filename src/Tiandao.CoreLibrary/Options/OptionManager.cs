﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Tiandao.Options
{
    public class OptionManager : IOptionProvider
	{
		#region 私有枚举

		private enum InvokeMethod
		{
			Apply,
			Reset,
		}

		#endregion

		#region 单例实例

		public static readonly OptionManager Default = new OptionManager();

		#endregion

		#region 私有字段

		private OptionNode _root;
		private Collections.Collection<IOptionProvider> _providers;
		private IOptionLoaderSelector _loaderSelector;
		private HashSet<IOptionProvider> _unloadedProviders;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取选项管理的根节点。
		/// </summary>
		public OptionNode RootNode
		{
			get
			{
				return _root;
			}
		}

		/// <summary>
		/// 获取全局的<seealso cref="ISettingsProvider"/>选项设置，全局设置集的配置路径为“/Settings”。
		/// </summary>
		public ISettingsProvider Settings
		{
			get
			{
				var node = this.Find("/settings");

				if(node == null || node.Option == null)
					return null;

				return node.Option.OptionObject as ISettingsProvider;
			}
		}

		/// <summary>
		/// 获取<see cref="IOptionProvider"/>选项提供程序集合，该集合中的选项提供程序将由<seealso cref="LoaderSelector"/>属性指定的选项加载器驱动加载过程。
		/// </summary>
		public ICollection<IOptionProvider> Providers
		{
			get
			{
				return _providers;
			}
		}

		/// <summary>
		/// 获取或设置<see cref="IOptionLoaderSelector"/>选项加载选择器，默认为<seealso cref="OptionLoaderSelector"/>实例。
		/// </summary>
		public IOptionLoaderSelector LoaderSelector
		{
			get
			{
				if(_loaderSelector == null)
					System.Threading.Interlocked.CompareExchange(ref _loaderSelector, new OptionLoaderSelector(_root), null);

				return _loaderSelector;
			}
			set
			{
				_loaderSelector = value;
			}
		}

		#endregion

		#region 构造方法

		public OptionManager()
		{
			_root = new OptionNode();
			_unloadedProviders = new HashSet<IOptionProvider>();
			_providers = new Collections.Collection<IOptionProvider>();
			_providers.CollectionChanged += Providers_CollectionChanged;
		}

		#endregion

		#region 公共方法

		public void Apply()
		{
			//确认是否将所有待加载的配置都加载了
			this.EnsureLoadProviders();

			this.Invoke(_root, InvokeMethod.Apply);
		}

		public void Reset()
		{
			//确认是否将所有待加载的配置都加载了
			this.EnsureLoadProviders();

			this.Invoke(_root, InvokeMethod.Reset);
		}

		public object GetOptionObject(string path)
		{
			var node = this.Find(path);

			if(node != null && node.Option != null)
				return node.Option.OptionObject;

			return null;
		}

		public void SetOptionObject(string path, object optionObject)
		{
			var node = this.Find(path);

			if(node != null && node.Option != null && node.Option.Provider != null)
				node.Option.Provider.SetOptionObject(path, optionObject);
		}

		public OptionNode Find(string path)
		{
			if(string.IsNullOrWhiteSpace(path))
				return null;

			//确认是否将所有待加载的配置都加载了
			this.EnsureLoadProviders();

			return _root.Find(path);
		}

		public OptionNode Find(params string[] parts)
		{
			if(parts == null || parts.Length < 1)
				return null;

			//确认是否将所有待加载的配置都加载了
			this.EnsureLoadProviders();

			return _root.Find(parts);
		}

		#endregion

		#region 事件处理

		private void Providers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			var loaderSelector = this.LoaderSelector;

			if(loaderSelector == null)
				return;

			switch(e.Action)
			{
				case NotifyCollectionChangedAction.Reset:
				case NotifyCollectionChangedAction.Remove:
				case NotifyCollectionChangedAction.Replace:
					if(e.Action == NotifyCollectionChangedAction.Replace)
					{
						foreach(IOptionProvider provider in e.NewItems)
						{
							if(provider != null)
								_unloadedProviders.Add(provider);
						}
					}

					foreach(IOptionProvider provider in e.OldItems)
					{
						if(provider == null)
							continue;

						if(!_unloadedProviders.Contains(provider))
						{
							var loader = loaderSelector.GetLoader(provider);

							if(loader == null)
								loader.Unload(provider);
						}

						_unloadedProviders.Remove(provider);
					}

					break;
				case NotifyCollectionChangedAction.Add:
					foreach(IOptionProvider provider in e.NewItems)
					{
						if(provider != null)
							_unloadedProviders.Add(provider);
					}

					break;
			}
		}

		#endregion

		#region 私有方法

		private void Invoke(OptionNode node, InvokeMethod method)
		{
			if(node == null)
				return;

			if(node.Option != null)
			{
				switch(method)
				{
					case InvokeMethod.Apply:
						node.Option.Apply();
						break;
					case InvokeMethod.Reset:
						node.Option.Reset();
						break;
				}
			}

			foreach(var child in node.Children)
				this.Invoke(child, method);
		}

		private bool EnsureLoadProviders()
		{
			if(_unloadedProviders == null || _unloadedProviders.Count < 1)
				return false;

			var loaderSelector = this.LoaderSelector;

			if(loaderSelector == null)
				return false;

			var providers = new IOptionProvider[_unloadedProviders.Count];
			_unloadedProviders.CopyTo(providers);
			_unloadedProviders.Clear();

			foreach(var provider in providers)
			{
				if(provider == null)
					continue;

				var loader = loaderSelector.GetLoader(provider);

				if(loader != null)
					loader.Load(provider);
			}

			return true;
		}

		#endregion
	}
}
