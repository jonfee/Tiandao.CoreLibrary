using System;

namespace Tiandao.Serialization
{
	/// <summary>
	/// 表示序列化方向的枚举。
	/// </summary>
	public enum SerializationDirection
    {
		/// <summary>
		/// 输入，即反序列化调用。
		/// </summary>
		Input,

		/// <summary>
		/// 输出，即序列化调用。
		/// </summary>
		Output
	}
}
