using System;
using System.IO;

namespace Tiandao.Serialization
{
	/// <summary>
	/// 标识实现者实现的特定序列化程序。
	/// </summary>
	public interface ISerializable
    {
		/// <summary>
		/// 将当前实现类序列化到指定的流。
		/// </summary>
		/// <param name="serializationStream">序列化数据的流。此流可以引用多种后备存储区（如文件、网络、内存等）。</param>
		void Serialize(Stream serializationStream);
	}
}