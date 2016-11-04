using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public class ServiceResolvedEventArgs : EventArgs
	{
		#region 私有字段

		private string _serviceName;
		private Type _contractType;
		private object _parameter;
		private object _result;
		private bool _isResolveAll;

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

		public object Result
		{
			get
			{
				return _result;
			}
		}

		public bool IsResolveAll
		{
			get
			{
				return _isResolveAll;
			}
		}

		#endregion

		#region 构造方法

		public ServiceResolvedEventArgs(string serviceName, object result)
		{
			if(string.IsNullOrWhiteSpace(serviceName))
				throw new ArgumentNullException("serviceName");

			_serviceName = serviceName.Trim();
			_isResolveAll = false;
			_result = result;
		}

		public ServiceResolvedEventArgs(Type contractType, object parameter, bool isResolveAll, object result)
		{
			if(contractType == null)
				throw new ArgumentNullException("contractType");

			_contractType = contractType;
			_parameter = parameter;
			_isResolveAll = isResolveAll;
			_result = result;
		}

		#endregion
	}
}
