using System;
using System.Collections.Generic;

namespace Tiandao.Caching
{
	/// <summary>
	/// 表示缓存项发生改变的原因。
	/// </summary>
	public enum CacheChangedReason
	{
		/// <summary>
		/// 系统内部原因。
		/// </summary>
		None,

		/// <summary>
		/// 被手动删除的。
		/// </summary>
		Removed,

		/// <summary>
		/// 被手动更新的。
		/// </summary>
		Updated,

		/// <summary>
		/// 因为过期被删除。
		/// </summary>
		Expired,

		/// <summary>
		/// 因为依赖被删除。
		/// </summary>
		Dependented,
	}
}