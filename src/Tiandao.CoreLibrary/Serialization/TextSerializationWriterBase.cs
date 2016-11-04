﻿using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Reflection;

using Tiandao.Common;

namespace Tiandao.Serialization
{
    public abstract class TextSerializationWriterBase : ISerializationWriter
	{
		#region 事件定义

		public event EventHandler<SerializationWritingEventArgs> Writing;
		public event EventHandler<SerializationWroteEventArgs> Wrote;

		#endregion

		#region 静态字段

		private static IDictionary<SerializationContext, TextWriter> _writers;

		#endregion

		#region 成员字段

		private Encoding _encoding;
		private string _indentString;

		#endregion

		#region 公共属性

		public Encoding Encoding
		{
			get
			{
				return _encoding;
			}
			set
			{
				_encoding = value ?? Encoding.UTF8;
			}
		}

		public string IndentString
		{
			get
			{
				return _indentString;
			}
			set
			{
				_indentString = value ?? string.Empty;
			}
		}

		#endregion

		#region 构造方法

		protected TextSerializationWriterBase()
		{
			_encoding = Encoding.UTF8;
			_indentString = "	";
		}

		#endregion

		#region 公共方法

		public void Write(SerializationWriterContext context)
		{
			//激发“Writing”事件
			if(this.OnWriting(context))
				return;

			//执行写入操作
			this.OnWrite(context);

			//激发“Wrote”事件
			this.OnWrote(new SerializationWroteEventArgs(context));
		}

		#endregion

		#region 抽象方法

		protected abstract void OnWrite(SerializationWriterContext context);

		#endregion

		#region 显式实现

		void ISerializationWriter.OnSerializing(SerializationContext context)
		{
			this.OnSerializing(context);
		}

		void ISerializationWriter.OnSerialized(SerializationContext context)
		{
			this.OnSerialized(context);

			var writers = _writers;
			if(writers != null)
				writers.Remove(context);
		}

		void ISerializationWriter.OnWrote(SerializationWriterContext context)
		{
			this.OnWrote(context);
		}

		#endregion

		#region 虚拟方法

		protected virtual void OnSerializing(SerializationContext context)
		{
		}

		protected virtual void OnSerialized(SerializationContext context)
		{
			var writer = this.GetWriter(context);

			if(writer != null)
			{
				writer.Flush();
				writer.Dispose();
			}
		}

		protected virtual void OnWrote(SerializationWriterContext context)
		{
		}

		protected virtual TextWriter CreateTextWriter(SerializationContext context)
		{
			return new StreamWriter(context.SerializationStream, _encoding, 1024 * 32, true);
		}

		protected virtual bool GetDirectedValue(object value, out string valueText)
		{
			valueText = null;

			if(value == null)
				return true;

			Type valueType = value.GetType();
			bool isDirectedValue = valueType.IsPrimitive() || valueType.IsEnum() ||
								   valueType == typeof(decimal) || valueType == typeof(DateTime) || valueType == typeof(DBNull) ||
								   valueType == typeof(object) || valueType == typeof(string) || valueType == typeof(Guid) ||
								   TypeExtension.IsAssignableFrom(typeof(Type), valueType);

			if(isDirectedValue)
			{
				valueText = value.ToString();
				return true;
			}

#if !CORE_CLR
			var converter = TypeDescriptor.GetConverter(value);
#else
			var converter = TypeDescriptor.GetConverter(value.GetType());
#endif

			if(converter != null && converter.GetType() != typeof(TypeConverter))
			{
				isDirectedValue = converter.CanConvertFrom(typeof(string)) && converter.CanConvertTo(typeof(string));

				if(isDirectedValue)
					valueText = converter.ConvertToInvariantString(value);
			}

			return isDirectedValue;
		}

		#endregion

		#region 激发事件

		private bool OnWriting(SerializationWriterContext context)
		{
			//创建事件参数对象
			var args = new SerializationWritingEventArgs(context);

			//激发“Writing”事件
			this.OnWriting(args);

			return args.Cancel;
		}

		protected virtual void OnWriting(SerializationWritingEventArgs args)
		{
			var e = this.Writing;

			if(e != null)
				e(this, args);
		}

		protected virtual void OnWrote(SerializationWroteEventArgs args)
		{
			var e = this.Wrote;

			if(e != null)
				e(this, args);
		}

		#endregion

		#region 保护方法

		protected TextWriter GetWriter(SerializationWriterContext context)
		{
			if(context == null)
				return null;

			return this.GetWriter(context.SerializationContext);
		}

		protected TextWriter GetWriter(SerializationContext context)
		{
			if(context == null)
				return null;

			if(_writers == null)
				System.Threading.Interlocked.CompareExchange(ref _writers, new ConcurrentDictionary<SerializationContext, TextWriter>(), null);

			TextWriter writer;

			if(!_writers.TryGetValue(context, out writer))
			{
				writer = this.CreateTextWriter(context);

				if(writer != null)
					_writers.Add(context, writer);
			}

			return writer;
		}

		protected string GetIndentText(int indent)
		{
			if(string.IsNullOrEmpty(_indentString) || indent < 1)
				return string.Empty;

			string result = string.Empty;

			for(int i = 0; i < indent; i++)
				result += _indentString;

			return result;
		}

		#endregion
	}
}
