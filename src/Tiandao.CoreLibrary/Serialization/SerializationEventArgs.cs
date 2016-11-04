using System;
using System.IO;

namespace Tiandao.Serialization
{
#if !CORE_CLR
	[Serializable]
#endif
	public class SerializationEventArgs : EventArgs
    {
		#region 私有字段

		private SerializationDirection _direction;
		private Stream _serializationStream;
		private object _serializationObject;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取当前序列化的调用方向。
		/// </summary>
		public SerializationDirection Direction
		{
			get
			{
				return _direction;
			}
		}

		/// <summary>
		/// 获取序列化流对象。
		/// </summary>
		public Stream SerializationStream
		{
			get
			{
				return _serializationStream;
			}
		}

		/// <summary>
		/// 获取序列化对象。
		/// </summary>
		public object SerializationObject
		{
			get
			{
				return _serializationObject;
			}
		}

		#endregion

		#region 构造方法

		public SerializationEventArgs(SerializationDirection direction, Stream serializationStream, object serializationObject)
		{
			if(serializationStream == null)
				throw new ArgumentNullException(nameof(serializationStream));

			_direction = direction;
			_serializationStream = serializationStream;
			_serializationObject = serializationObject;
		}

		#endregion
	}
}
