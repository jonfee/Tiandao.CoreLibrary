using System;
using System.ComponentModel;

namespace Tiandao.Services
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
	public class ServiceDependencyAttribute : Attribute
	{
		#region 私有字段

		private string _name;
		private Type _contract;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取或设置服务的名称。
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
			}
		}

		/// <summary>
		/// 获取或设置服务的契约类型。
		/// </summary>
		public Type Contract
		{
			get
			{
				return _contract;
			}
			set
			{
				_contract = value;
			}
		}

		#endregion

		#region 构造方法

		public ServiceDependencyAttribute()
		{
		}

		public ServiceDependencyAttribute(string name)
		{
			this.Name = name;
		}

		public ServiceDependencyAttribute(Type contract)
		{
			this.Contract = contract;
		}

		#endregion
	}
}
