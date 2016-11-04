using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public class ServiceProvider : ServiceProviderBase
	{
		#region 构造方法

		public ServiceProvider()
		{
			this.Storage = new ServiceStorage(this);
		}

		public ServiceProvider(IServiceBuilder builder)
		{
			this.Builder = builder;
			this.Storage = new ServiceStorage(this);
		}

		public ServiceProvider(IServiceStorage storage, IServiceBuilder builder) : base(storage, builder)
		{

		}

		#endregion
	}
}
