using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public interface IServiceProvider : System.IServiceProvider
    {
		#region 事件定义

		event EventHandler<ServiceRegisteredEventArgs> Registered;
		event EventHandler<ServiceUnregisteredEventArgs> Unregistered;

		#endregion

		#region 属性定义

		IServiceStorage Storage
		{
			get;
		}

		#endregion

		#region 注册方法

		void Register(string name, Type serviceType);
		void Register(string name, Type serviceType, params Type[] contractTypes);

		void Register(string name, object service);
		void Register(string name, object service, params Type[] contractTypes);

		void Register(Type serviceType, params Type[] contractTypes);
		void Register(object service, params Type[] contractTypes);

		void Unregister(string name);

		#endregion

		#region 解析方法

		/// <summary>
		/// 获取指定名称的服务类型。
		/// </summary>
		/// <param name="name">指定的服务名称。</param>
		/// <returns>返回指定名称的服务的类型，如果返回空(null)则表示没有找到指定名称的服务。</returns>
		Type GetServiceType(string name);

		object Resolve(string name);
		object Resolve(Type type);
		object Resolve(Type type, object parameter);

		object ResolveRequired(string name);
		object ResolveRequired(Type type);
		object ResolveRequired(Type type, object parameter);

		T Resolve<T>() where T : class;
		T Resolve<T>(object parameter) where T : class;

		T ResolveRequired<T>() where T : class;
		T ResolveRequired<T>(object parameter) where T : class;

		IEnumerable<object> ResolveAll(Type type);
		IEnumerable<object> ResolveAll(Type type, object parameter);

		IEnumerable<T> ResolveAll<T>() where T : class;
		IEnumerable<T> ResolveAll<T>(object parameter) where T : class;

		#endregion
	}
}
