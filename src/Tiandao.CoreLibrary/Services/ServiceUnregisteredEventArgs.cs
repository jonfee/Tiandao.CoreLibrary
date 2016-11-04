using System;

namespace Tiandao.Services
{
    public class ServiceUnregisteredEventArgs : EventArgs
    {
		#region 私有字段

		private ServiceEntry _entry;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取注册完成的<see cref="ServiceEntry"/>服务描述项对象。
		/// </summary>
		public ServiceEntry Entry
		{
			get
			{
				return _entry;
			}
		}

		#endregion

		#region 构造方法

		public ServiceUnregisteredEventArgs(ServiceEntry entry)
		{
			_entry = entry;
		}

		#endregion
	}
}
