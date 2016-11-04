﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Tiandao.Common;

namespace Tiandao.Services
{
#if !CORE_CLR
	public abstract class ServiceStorageBase : MarshalByRefObject, IServiceStorage, IEnumerable<ServiceEntry>
#else
	public abstract class ServiceStorageBase : IServiceStorage, IEnumerable<ServiceEntry>
#endif
	{
		#region 私有字段

		private IMatcher _matcher;
		private IServiceProvider _provider;

		#endregion

		#region 公共属性

		public abstract int Count
		{
			get;
		}

		public IMatcher Matcher
		{
			get
			{
				return _matcher;
			}
			set
			{
				_matcher = value;
			}
		}

		public IServiceProvider Provider
		{
			get
			{
				return _provider;
			}
		}

		#endregion

		#region 构造方法

		protected ServiceStorageBase(IServiceProvider provider) : this(provider, Services.Matcher.Default)
		{
		}

		protected ServiceStorageBase(IServiceProvider provider, IMatcher matcher)
		{
			if(provider == null)
				throw new ArgumentNullException("provider");

			_matcher = matcher;
			_provider = provider;
		}

		#endregion

		#region 公共方法

		public ServiceEntry Add(object service)
		{
			return this.Add(service, null);
		}

		public ServiceEntry Add(object service, Type[] contractTypes)
		{
			var entry = new ServiceEntry(service, contractTypes);
			this.Add(entry);
			return entry;
		}

		public ServiceEntry Add(Type serviceType)
		{
			return this.Add(serviceType, null);
		}

		public ServiceEntry Add(Type serviceType, Type[] contractTypes)
		{
			var entry = new ServiceEntry(serviceType, contractTypes);
			this.Add(entry);
			return entry;
		}

		public ServiceEntry Add(string name, object service)
		{
			return this.Add(name, service, null);
		}

		public ServiceEntry Add(string name, object service, Type[] contractTypes)
		{
			var entry = new ServiceEntry(name, service, contractTypes);
			this.Add(entry);
			return entry;
		}

		public ServiceEntry Add(string name, Type serviceType)
		{
			return this.Add(name, serviceType, null);
		}

		public ServiceEntry Add(string name, Type serviceType, Type[] contractTypes)
		{
			var entry = new ServiceEntry(name, serviceType, contractTypes);
			this.Add(entry);
			return entry;
		}

		public abstract void Add(ServiceEntry entry);

		public abstract void Clear();

		public abstract ServiceEntry Remove(string name);

		public virtual ServiceEntry Get(string name)
		{
			if(string.IsNullOrWhiteSpace(name))
				return null;

			//从当前容器及其外链容器中查找指定名称的服务
			var result = this.Find(name, new List<IServiceStorage>(new[] { this }));

			//如果上面的查找失败，则尝试从默认服务容器及其外链容器中查找指定名称的服务
			if(result == null && ServiceProviderFactory.Instance.Default != null && !object.ReferenceEquals(ServiceProviderFactory.Instance.Default, this))
				result = this.Find(name, new List<IServiceStorage>(new[] { ServiceProviderFactory.Instance.Default.Storage }));

			return result;
		}

		public virtual ServiceEntry Get(Type type, object parameter = null)
		{
			return (ServiceEntry)this.Find(type, parameter, false);
		}

		public virtual IEnumerable<ServiceEntry> GetAll(Type type, object parameter = null)
		{
			return (IEnumerable<ServiceEntry>)this.Find(type, parameter, true);
		}
		
		#endregion

		#region 匹配方法

		protected virtual bool OnMatch(ServiceEntry entry, object parameter)
		{
			if(entry == null)
				return false;

			var matcher = this.Matcher ?? Services.Matcher.Default;
			return matcher.Match(entry.Service, parameter);
		}

		#endregion

		#region 查找方法

		protected virtual object Find(Type type, object parameter, bool isMultiplex)
		{
			//从当前容器及其外链容器中查找指定类型的服务
			var result = Find(type, parameter, isMultiplex, new List<IServiceStorage>(new[] { this }));

			var succeed = result != null;

			if(succeed)
			{
				var entiries = result as ICollection<ServiceEntry>;
				succeed &= entiries == null || entiries.Count > 0;
			}

			//如果上面的查找失败，则尝试从默认服务容器及其外链容器中查找指定名称的服务
			if(!succeed && ServiceProviderFactory.Instance.Default != null && !object.ReferenceEquals(ServiceProviderFactory.Instance.Default, this))
				result = this.Find(type, parameter, isMultiplex, new List<IServiceStorage>(new[] { ServiceProviderFactory.Instance.Default.Storage }));

			return result;
		}

		private object Find(Type type, object parameter, bool isMultiplex, IList<IServiceStorage> storages)
		{
			if(type == null || storages == null)
				return null;

			var weakly = new List<ServiceEntry>();
			var strong = new HashSet<ServiceEntry>();

			for(int i = 0; i < storages.Count; i++)
			{
				var storage = storages[i];

				//获取当前容器的迭代器
				var enumerator = storage.GetEnumerator();

				//迭代查找服务，首先进行类型匹配然后再进行匹配比对
				while(enumerator.MoveNext())
				{
					var entry = enumerator.Current;

					if(entry == null || entry.ServiceType == null)
						continue;

					//如果服务条目声明了契约，则按契约声明进行匹配
					if(entry.HasContracts)
					{
						//契约的严格匹配
						if(entry.ContractTypes.Contains(type) && this.OnMatch(entry, parameter))
						{
							if(!isMultiplex)
								return entry;

							strong.Add(entry);
						}
						else //契约的弱匹配
						{
							foreach(var contract in entry.ContractTypes)
							{
								if(type.IsAssignableFrom(contract) && this.OnMatch(entry, parameter))
									weakly.Add(entry);
							}
						}
					}
					else //处理未声明契约的服务
					{
						//服务类型的严格匹配
						if(entry.ServiceType == type && this.OnMatch(entry, parameter))
						{
							if(!isMultiplex)
								return entry;

							strong.Add(entry);
						}
						else //服务类型的弱匹配
						{
							if(type.IsAssignableFrom(entry.ServiceType) && this.OnMatch(entry, parameter))
								weakly.Add(entry);
						}
					}

					//如果只查找单个服务
					if(!isMultiplex)
					{
						//如果只查找单个服务，并且弱匹配已成功则退出查找
						if(weakly.Count > 0)
							break;

						//如果当前服务项是一个服务容器
						if(typeof(IServiceProvider).IsAssignableFrom(entry.ServiceType))
						{
							var provider = (IServiceProvider)entry.Service;

							//如果当前服务项对应的服务容器不在外部容器列表中，则将当前服务项(服务容器)加入到外部服务容器列表中
							if(provider != null && !storages.Contains(provider.Storage))
								storages.Add(provider.Storage);
						}
					}
				}

				if(isMultiplex)
					return strong.Union(weakly).ToArray();
				else if(weakly.Count > 0)
					return weakly[0];
			}

			//返回空(查找失败)
			return null;
		}

		private ServiceEntry Find(string name, IList<IServiceStorage> storages)
		{
			if(string.IsNullOrWhiteSpace(name) || storages == null)
				return null;

			for(int i = 0; i < storages.Count; i++)
			{
				var storage = storages[i];

				//获取当前容器的迭代器
				var enumerator = storage.GetEnumerator();

				//迭代查找服务
				while(enumerator.MoveNext())
				{
					var entry = enumerator.Current;

					if(entry == null)
						continue;

					//如果名称匹配成功则返回（名称不区分大小写）
					if(string.Equals(entry.Name, name, StringComparison.OrdinalIgnoreCase))
						return entry;

					//如果当前服务项是一个服务容器
					if(entry.ServiceType != null && typeof(IServiceProvider).IsAssignableFrom(entry.ServiceType))
					{
						var provider = (IServiceProvider)entry.Service;

						//如果当前服务项对应的服务容器不在外部容器列表中，则将当前服务项(服务容器)加入到外部服务容器列表中
						if(provider != null && !storages.Contains(provider.Storage))
							storages.Add(provider.Storage);
					}
				}
			}

			//返回空(查找失败)
			return null;
		}

		#endregion

		#region 枚举遍历

		public abstract IEnumerator<ServiceEntry> GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}