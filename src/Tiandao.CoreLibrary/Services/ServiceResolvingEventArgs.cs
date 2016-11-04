using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Tiandao.Services
{
    public class ServiceResolvingEventArgs : CancelEventArgs
	{
		#region 私有字段

		private string _serviceName;
		private Type _contractType;
		private object _parameter;
		private bool _isResolveAll;
		private object _result;

		#endregion

		#region 公共属性

		public string ServiceName
		{
			get
			{
				return _serviceName;
			}
		}

		public Type ContractType
		{
			get
			{
				return _contractType;
			}
		}

		public object Parameter
		{
			get
			{
				return _parameter;
			}
		}

		public bool IsResolveAll
		{
			get
			{
				return _isResolveAll;
			}
		}

		public object Result
		{
			get
			{
				return _result;
			}
			set
			{
				_result = value;
			}
		}

		#endregion

		#region 构造方法

		public ServiceResolvingEventArgs(string serviceName)
		{
			if(string.IsNullOrWhiteSpace(serviceName))
				throw new ArgumentNullException("serviceName");

			_serviceName = serviceName.Trim();
			_isResolveAll = false;
		}

		public ServiceResolvingEventArgs(Type contractType, object parameter, bool isResolveAll)
		{
			if(contractType == null)
				throw new ArgumentNullException("contractType");

			_contractType = contractType;
			_parameter = parameter;
			_isResolveAll = isResolveAll;
		}

		#endregion
	}
}