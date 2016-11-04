using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Tiandao.Services
{
#if !CORE_CLR
	public class ServiceProviderFactory : MarshalByRefObject, IServiceProviderFactory, IEnumerable<KeyValuePair<string, IServiceProvider>>
#else
	public class ServiceProviderFactory : IServiceProviderFactory, IEnumerable<KeyValuePair<string, IServiceProvider>>
#endif
	{
		#region 单例字段

		private static ServiceProviderFactory _instance;

		#endregion

		#region 单例属性

		public static ServiceProviderFactory Instance
		{
			get
			{
				if(_instance == null)
					System.Threading.Interlocked.CompareExchange(ref _instance, new ServiceProviderFactory(), null);

				return _instance;
			}
			set
			{
				_instance = value;
			}
		}

		#endregion

		#region 私有字段

		private string _defaultName;
		private ConcurrentDictionary<string, IServiceProvider> _providers;

		#endregion

		#region 公共属性

		public int Count
		{
			get
			{
				return _providers.Count;
			}
		}

		public virtual IServiceProvider Default
		{
			get
			{
				return this.GetProvider(_defaultName);
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException();

				this.Register(_defaultName, value);
			}
		}

		#endregion

		#region 保护属性

		protected string DefaultName
		{
			get
			{
				return _defaultName;
			}
		}

		#endregion

		#region 构造方法

		protected ServiceProviderFactory() : this(string.Empty)
		{
		}

		protected ServiceProviderFactory(string defaultName)
		{
			_defaultName = string.IsNullOrWhiteSpace(defaultName) ? string.Empty : defaultName.Trim();
			_providers = new ConcurrentDictionary<string, IServiceProvider>(StringComparer.OrdinalIgnoreCase);
		}

		#endregion

		#region 公共方法

		public void Register(string name, IServiceProvider provider)
		{
			this.Register(name, provider, true);
		}

		public bool Register(string name, IServiceProvider provider, bool replaceOnExists)
		{
			if(provider == null)
				throw new ArgumentNullException("provider");

			name = string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim();

			if(replaceOnExists)
				_providers.AddOrUpdate(name, provider, (_, __) => provider);
			else
				return _providers.TryAdd(name, provider);

			//返回成功
			return true;
		}

		/// <summary>
		/// 注销服务供应程序。
		/// </summary>
		/// <param name="name">要注销服务供应程序的名称。</param>
		public void Unregister(string name)
		{
			IServiceProvider temp;
			_providers.TryRemove(string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim(), out temp);
		}

		/// <summary>
		/// 获取指定名称的服务供应程序。具体的获取策略请参考更详细的备注说明。
		/// </summary>
		/// <param name="name">待获取的服务供应程序名。</param>
		/// <returns>如果指定名称的供应程序回存在则返它，否则返回空(null)。</returns>
		public virtual IServiceProvider GetProvider(string name)
		{
			IServiceProvider result;

			if(_providers.TryGetValue(string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim(), out result))
				return result;

			return null;
		}

		#endregion

		#region 显式实现

		IEnumerator<KeyValuePair<string, IServiceProvider>> IEnumerable<KeyValuePair<string, IServiceProvider>>.GetEnumerator()
		{
			return _providers.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _providers.GetEnumerator();
		}

		#endregion
	}
}