using System;

namespace Tiandao.Services
{
    public class ServiceEntry
    {
		#region 私有字段

		private string _name;
		private object _service;
		private Type _serviceType;
		private Type[] _contractTypes;
		private object _userToken;

		private IServiceBuilder _builder;
		private IServiceLifetime _lifetime;

		private readonly object _syncRoot = new object();

		#endregion

		#region 公共属性

		public virtual string Name
		{
			get
			{
				return _name;
			}
		}

		public virtual Type ServiceType
		{
			get
			{
				if(_serviceType == null)
				{
					object instance = this.Service;

					if(instance != null)
						_serviceType = instance.GetType();
				}

				return _serviceType;
			}
		}

		public virtual object Service
		{
			get
			{
				var result = _service;

				if(result == null)
				{
					lock (_syncRoot)
					{
						if(_service == null)
						{
							//创建一个新的服务实例
							_service = this.CreateService();

							return _service;
						}
					}
				}

				var lifetime = _lifetime;

				//如果没有指定服务的生命期或者当前服务是可用的则返回它
				if(lifetime == null || lifetime.IsAlive(this))
					return result;

				//至此，表明当前服务已被判定过期不可用，则重新创建一个新的服务实例(并确保当前服务没有被修改过)
				System.Threading.Interlocked.CompareExchange(ref _service, this.CreateService(), result);

				return _service;
			}
		}

		public bool HasService
		{
			get
			{
				return _service != null;
			}
		}

		public bool HasContracts
		{
			get
			{
				return _contractTypes != null && _contractTypes.Length > 0;
			}
		}

		public virtual Type[] ContractTypes
		{
			get
			{
				return _contractTypes;
			}
		}

		public object UserToken
		{
			get
			{
				return _userToken;
			}
			set
			{
				_userToken = value;
			}
		}

		public IServiceBuilder Builder
		{
			get
			{
				return _builder;
			}
			set
			{
				_builder = value;
			}
		}

		public IServiceLifetime Lifetime
		{
			get
			{
				return _lifetime;
			}
			set
			{
				_lifetime = value;
			}
		}

		#endregion

		#region 构造方法

		protected ServiceEntry()
		{
		}

		public ServiceEntry(string name, object service, Type[] contractTypes, object userToken = null)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			if(service == null)
				throw new ArgumentNullException("service");

			_name = name.Trim();
			_service = service;
			_serviceType = service.GetType();
			_contractTypes = contractTypes;
			_userToken = userToken;
		}

		public ServiceEntry(string name, Type serviceType, Type[] contractTypes, object userToken = null)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			if(serviceType == null)
				throw new ArgumentNullException("serviceType");

			_name = name.Trim();
			_serviceType = serviceType;
			_contractTypes = contractTypes;
			_userToken = userToken;
		}

		public ServiceEntry(object service, Type[] contractTypes, object userToken = null)
		{
			if(service == null)
				throw new ArgumentNullException("service");

			_service = service;
			_serviceType = service.GetType();
			_contractTypes = contractTypes;
			_userToken = userToken;
		}

		public ServiceEntry(Type serviceType, Type[] contractTypes, object userToken = null)
		{
			if(serviceType == null)
				throw new ArgumentNullException("serviceType");

			_serviceType = serviceType;
			_contractTypes = contractTypes;
			_userToken = userToken;
		}

		#endregion

		#region 虚拟方法

#if !CORE_CLR
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
#endif
		protected virtual object CreateService()
		{
			var builder = _builder;

			if(builder != null)
			{
				var instance = builder.Build(this);

				if(instance != null)
					_serviceType = instance.GetType();

				return instance;
			}

			var type = _serviceType;

			if(type != null)
				return Activator.CreateInstance(type);

			return null;
		}

		#endregion

		#region 重写方法

		public override string ToString()
		{
			if(string.IsNullOrWhiteSpace(this.Name))
				return this.ServiceType.FullName;
			else
				return string.Format("{0} ({1})", this.Name, this.ServiceType.FullName);
		}

		public override int GetHashCode()
		{
			var instance = this.Service;
			return instance == null ? 0 : instance.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if(obj == null || obj.GetType() != typeof(ServiceEntry))
				return false;

			return object.ReferenceEquals(this.Service, ((ServiceEntry)obj).Service);
		}

		#endregion
	}
}
