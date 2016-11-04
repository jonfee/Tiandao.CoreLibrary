using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
	public interface IServiceStorage : IEnumerable<ServiceEntry>
	{
		#region 属性定义

		IMatcher Matcher
		{
			get;
			set;
		}

		#endregion

		#region 方法定义

		void Clear();
		void Add(ServiceEntry entry);
		ServiceEntry Remove(string name);

		ServiceEntry Get(string name);
		ServiceEntry Get(Type type, object parameter = null);
		IEnumerable<ServiceEntry> GetAll(Type type, object parameter = null);

		#endregion
	}
}
