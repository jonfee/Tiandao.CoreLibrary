using System;
using System.Collections.Generic;

using Xunit;
using Tiandao.Caching;
using Tiandao.Test;

namespace Tiandao.Services.Test
{
    public class ServiceProviderTest
    {
		#region 私有字段

		private ServiceProvider _provider1;
		private ServiceProvider _provider2;
		private ServiceProvider _provider3;

		#endregion

		#region 构造方法

		public ServiceProviderTest()
		{
			_provider1 = new ServiceProvider();
			_provider2 = new ServiceProvider();
			_provider3 = new ServiceProvider();

			ServiceProviderFactory.Instance.Default = new ServiceProvider();
			ServiceProviderFactory.Instance.Default.Register("string", "I'm a service.");

			_provider1.Register("MC1", new MemoryCache("MemoryCache-1"), typeof(ICache));
			_provider1.Register(typeof(Address));

			_provider2.Register("MC2", new MemoryCache("MemoryCache-2"), typeof(ICache));
			_provider2.Register(typeof(Department));

			_provider3.Register("MC3", new MemoryCache("MemoryCache-3"), typeof(ICache));
			_provider3.Register(typeof(Person));
		}

		#endregion

		#region 测试方法

		[Fact]
	    public void ResolveTest()
	    {
			ICache cache = null;

			cache = _provider1.Resolve<ICache>();
			Assert.NotNull(cache);
			Assert.Equal("MemoryCache-1", cache.Name);

			cache = _provider1.Resolve("MC1") as ICache;
			Assert.NotNull(cache);
			Assert.Equal("MemoryCache-1", cache.Name);

			cache = _provider1.Resolve("MC2") as ICache;
			Assert.Null(cache);

			Assert.NotNull(_provider1.Resolve("string"));
			Assert.IsAssignableFrom(typeof(string), _provider1.Resolve("string"));
			Assert.NotNull(_provider2.Resolve<string>());
			Assert.NotNull(_provider3.Resolve<string>());
			Assert.IsAssignableFrom(typeof(string), _provider3.Resolve("string"));

			//将二号服务容器加入到一号服务容器中
			_provider1.Register(_provider2);

			cache = _provider1.Resolve("MC2") as ICache;
			Assert.NotNull(cache);
			Assert.Equal("MemoryCache-2", cache.Name);

			cache = _provider1.Resolve("MC3") as ICache;
			Assert.Null(cache);

			//将三号服务容器加入到二号服务容器中
			_provider2.Register(_provider3);

			//将一号服务容器加入到三号服务容器中（形成循环链）
			_provider3.Register(_provider1);

			cache = _provider1.Resolve("MC3") as ICache;
			Assert.NotNull(cache);
			Assert.Equal("MemoryCache-3", cache.Name);

			var address = _provider1.Resolve<Address>();
			Assert.NotNull(address);

			var department = _provider1.Resolve<Department>();
			Assert.NotNull(department);

			var person = _provider1.Resolve<Person>();
			Assert.NotNull(person);

			Assert.NotNull(_provider1.Resolve("string"));
			Assert.IsAssignableFrom(typeof(string), _provider1.Resolve("string"));
			Assert.NotNull(_provider2.Resolve<string>());
			Assert.NotNull(_provider3.Resolve<string>());
			Assert.IsAssignableFrom(typeof(string), _provider3.Resolve("string"));

			//测试不存在的服务
			Assert.Null(_provider1.Resolve<IWorker>());
			Assert.Null(_provider1.Resolve("NoExisted"));
		}

	    #endregion
	}
}
