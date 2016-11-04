using System;
using System.Reflection;
using System.Collections.Concurrent;
using Tiandao.Common;

namespace Tiandao.Options
{
    public class OptionLoaderSelector : IOptionLoaderSelector
	{
		#region 私有字段

		private OptionNode _root;
		private readonly ConcurrentDictionary<Type, IOptionLoader> _loaders;

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

		public OptionLoaderSelector(OptionNode root)
		{
			if(root == null)
				throw new ArgumentNullException(nameof(root));

			_root = root;
			_loaders = new ConcurrentDictionary<Type, IOptionLoader>();
		}

		#endregion

		#region 公共方法

		public void Register(Type providerType, IOptionLoader loader)
		{
			if(providerType == null)
				throw new ArgumentNullException(nameof(providerType));

			if(loader == null)
				throw new ArgumentNullException(nameof(loader));

			_loaders[providerType] = loader;
		}

		public void Register(Type providerType, Type loaderType)
		{
			if(providerType == null)
				throw new ArgumentNullException(nameof(providerType));

			if(loaderType == null)
				throw new ArgumentNullException(nameof(loaderType));

			var loader = this.CreateLoader(loaderType);

			if(loader == null)
				throw new InvalidOperationException("Can't create a instance with the loaderType.");

			_loaders[providerType] = loader;
		}

		public IOptionLoader GetLoader(IOptionProvider provider)
		{
			if(provider == null)
				return null;

			IOptionLoader loader;
			var providerType = provider.GetType();

			if(_loaders.TryGetValue(providerType, out loader) && loader != null)
				return loader;

			var attribute = (OptionLoaderAttribute)providerType.GetCustomAttribute(typeof(OptionLoaderAttribute), true);

			if(attribute != null && attribute.LoaderType != null)
			{
				loader = this.CreateLoader(attribute.LoaderType);

				if(loader != null)
				{
					if(_loaders.TryAdd(providerType, loader))
						return loader;

					return _loaders[providerType];
				}
			}

			return null;
		}

		#endregion

		#region 虚拟方法

		protected virtual IOptionLoader CreateLoader(Type type)
		{
			if(type == null)
				return null;

			if(!TypeExtension.IsAssignableFrom(typeof(IOptionLoader), type))
				throw new ArgumentException("The parameter is not a IOptionLoader type.");

#if !CORE_CLR
			var constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(OptionNode) }, null);
#else
			var constructor = type.GetConstructor(new Type[] { typeof(OptionNode) });
#endif

			try
			{
				if(constructor != null)
					return (IOptionLoader)constructor.Invoke(new object[] { _root });

				return (IOptionLoader)Activator.CreateInstance(type, true);
			}
			catch(Exception ex)
			{
				throw new InvalidOperationException(string.Format("Can create a option-loader instance from the '{0}' type.", type), ex);
			}
		}

		#endregion
	}
}
