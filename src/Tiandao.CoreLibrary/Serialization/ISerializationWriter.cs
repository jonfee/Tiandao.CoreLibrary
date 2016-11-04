using System;

namespace Tiandao.Serialization
{
    public interface ISerializationWriter
    {
		event EventHandler<SerializationWritingEventArgs> Writing;
		event EventHandler<SerializationWroteEventArgs> Wrote;

		void OnSerializing(SerializationContext context);
		void OnSerialized(SerializationContext context);
		void OnWrote(SerializationWriterContext context);

		/// <summary>
		/// 根据序列化的<seealso cref="SerializationWriterContext"/>上下文对象执行具体的写入操作。
		/// </summary>
		/// <param name="context">执行序列化操作的上下文。</param>
		/// <returns>如果当前写入的对象后不再需要进行后续的成员序列化写入则可以设置<paramref name="context"/>参数指定的<seealso cref="SerializationWriterContext.Terminated"/>属性为真(True)。</returns>
		void Write(SerializationWriterContext context);
	}
}
