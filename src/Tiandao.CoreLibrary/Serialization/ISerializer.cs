using System;
using System.IO;

namespace Tiandao.Serialization
{
	/// <summary>
	/// 提供将对象序列化到流中和从流中反序列化对象的功能。
	/// </summary>
	public interface ISerializer
    {
		/// <summary>
		/// 获取当前序列化的默认选项设置。
		/// </summary>
		SerializationSettings Settings
		{
			get;
		}

		/// <summary>
		/// 反序列化指定<paramref name="serializationStream"/>包含的对象。
		/// </summary>
		/// <param name="serializationStream">待反序列化的流。</param>
		/// <returns>反序列化的结果。</returns>
		object Deserialize(Stream serializationStream);

		/// <summary>
		/// 反序列化指定<paramref name="serializationStream"/>包含的对象。
		/// </summary>
		/// <param name="serializationStream">待反序列化的流。</param>
		/// <param name="type">反序列化的结果类型。</param>
		/// <returns>反序列化的结果。</returns>
		object Deserialize(Stream serializationStream, Type type);

		/// <summary>
		/// 反序列化指定<paramref name="serializationStream"/>包含的对象。
		/// </summary>
		/// <typeparam name="T">指定的反序列化结果的泛类型。</typeparam>
		/// <param name="serializationStream">待反序列化的流。</param>
		/// <returns>反序列化的结果。</returns>
		T Deserialize<T>(Stream serializationStream);

		/// <summary>
		/// 将指定的对象序列化到指定的<seealso cref="System.IO.Stream"/>流中。
		/// </summary>
		/// <param name="serializationStream">要将对象序列化到的流。</param>
		/// <param name="graph">待序列化的目标对象。</param>
		/// <param name="settings">序列化的选项设置。</param>
		void Serialize(Stream serializationStream, object graph, SerializationSettings settings = null);
	}
}
