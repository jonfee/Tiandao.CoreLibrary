using System;
using System.Reflection;

namespace Tiandao.Serialization
{
#if !CORE_CLR
	public class SerializationWriterContext : MarshalByRefObject
#else
	public class SerializationWriterContext
#endif
	{
		#region 私有字段

		private SerializationContext _serializationContext;
		private ISerializationWriter _writer;
		private object _value;
		private object _container;
		private MemberInfo _member;
		private int _index;
		private int _depth;
		private bool _isCircularReference;
		private bool _isCollection;
		private bool _terminated;

		#endregion

		#region 公共属性

		public SerializationContext SerializationContext
		{
			get
			{
				return _serializationContext;
			}
		}

		public SerializationSettings Settings
		{
			get
			{
				return _serializationContext.Settings;
			}
		}

		public ISerializationWriter Writer
		{
			get
			{
				return _writer;
			}
		}

		public int Depth
		{
			get
			{
				return _depth;
			}
		}

		public int Index
		{
			get
			{
				return _index;
			}
		}

		public object Value
		{
			get
			{
				return _value;
			}
		}

		public object Container
		{
			get
			{
				return _container;
			}
		}

		public MemberInfo Member
		{
			get
			{
				return _member;
			}
		}

		public string MemberName
		{
			get
			{
				return _member == null ? null : _member.Name;
			}
		}

		public Type MemberType
		{
			get
			{
				if(_member != null)
				{
					if(_member is FieldInfo)
					{
						return ((FieldInfo)_member).FieldType;
					}
					else if(_member is PropertyInfo)
					{
						return ((PropertyInfo)_member).PropertyType;
					}

//					switch(_member.MemberType)
//					{
//						case MemberTypes.Field:
//							return ((FieldInfo)_member).FieldType;
//						case MemberTypes.Property:
//							return ((PropertyInfo)_member).PropertyType;
//					}
				}

				return null;
			}
		}

		public bool IsCollection
		{
			get
			{
				return _isCollection;
			}
		}

		public bool IsCircularReference
		{
			get
			{
				return _isCircularReference;
			}
		}

		public bool Terminated
		{
			get
			{
				return _terminated;
			}
			set
			{
				_terminated = value;
			}
		}

		#endregion

		#region 构造方法

		public SerializationWriterContext(ISerializationWriter writer, SerializationContext serializationContext, object container, object value, MemberInfo member, int index, int depth, bool isCircularReference, bool isCollection)
		{
			if(writer == null)
				throw new ArgumentNullException(nameof(writer));

			if(serializationContext == null)
				throw new ArgumentNullException(nameof(serializationContext));

			_writer = writer;
			_serializationContext = serializationContext;
			_value = value;
			_index = index;
			_depth = depth;
			_member = member;
			_container = container;
			_terminated = value == null || value.GetType().GetTypeInfo().IsPrimitive;
			_isCircularReference = isCircularReference;
			_isCollection = isCollection;
		}
		
		#endregion
	}
}