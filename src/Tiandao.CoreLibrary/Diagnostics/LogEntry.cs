using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Tiandao.Common;

namespace Tiandao.Diagnostics
{
#if !CORE_CLR
	public class LogEntry : MarshalByRefObject
#else
	public class LogEntry
#endif
	{
		#region 私有字段

		private LogLevel _level;
		private object _data;
		private string _source;
		private string _message;
		private string _stackTrace;
		private DateTime _timestamp;
		private Exception _exception;
		private string _toString;

		#endregion

		#region 公共属性

		public LogLevel Level
		{
			get
			{
				return _level;
			}
		}

		public string Source
		{
			get
			{
				return _source;
			}
		}

		public Exception Exception
		{
			get
			{
				return _exception;
			}
		}

		public string Message
		{
			get
			{
				return _message;
			}
		}

		public string StackTrace
		{
			get
			{
				return _stackTrace;
			}
			internal set
			{
				_stackTrace = value;
				_toString = null;
			}
		}

		public object Data
		{
			get
			{
				return _data;
			}
		}

		public DateTime Timestamp
		{
			get
			{
				return _timestamp;
			}
		}

		#endregion

		#region 构造方法

		public LogEntry(LogLevel level, string source, string message, object data = null) : this(level, source, message, null, data)
		{
		}

		public LogEntry(LogLevel level, string source, Exception exception, object data = null) : this(level, source, null, exception, data)
		{
		}

		public LogEntry(LogLevel level, string source, string message, Exception exception, object data = null)
		{
			_toString = null;
			_stackTrace = string.Empty;
			_source = string.IsNullOrEmpty(source) ? (exception == null ? string.Empty : exception.Source) : source.Trim();
			_exception = exception;
			_message = message ?? (exception == null ? string.Empty : exception.Message);
			_data = data ?? (exception != null && exception.Data != null && exception.Data.Count > 0 ? exception.Data : null);
			_level = level;
			_timestamp = DateTime.Now;
		}

		#endregion

		#region 重写方法

		public override string ToString()
		{
			if(string.IsNullOrEmpty(_toString))
			{
				StringBuilder builder = new StringBuilder(512);

				builder.AppendFormat("<log level=\"{0}\" source=\"{1}\" timestamp=\"{2}\">", _level.ToString().ToUpperInvariant(), _source, _timestamp);
				builder.AppendLine();
				builder.AppendLine("\t<message><![CDATA[" + _message + "]]></message>");

				var aggregateException = _exception as AggregateException;

				if(aggregateException != null)
				{
					if(aggregateException.InnerExceptions != null && aggregateException.InnerExceptions.Count > 0)
					{
						foreach(var exception in aggregateException.InnerExceptions)
							this.WriteException(builder, exception);
					}
					else
					{
						this.WriteException(builder, _exception);
					}
				}
				else
				{
					this.WriteException(builder, _exception);
				}

				if(_data != null)
				{
					builder.AppendLine();
					builder.AppendFormat("\t<data type=\"{0}, {1}\">" + Environment.NewLine, _data.GetType().FullName, _data.GetType().GetAssembly().GetName().Name);
					builder.AppendLine("\t<![CDATA[");

					byte[] bytes = _data as byte[];

					if(bytes == null)
						builder.AppendLine(Serialization.Serializer.Text.Serialize(_data));
					else
						builder.AppendLine(Common.Converter.ToHexString(bytes));

					builder.AppendLine("\t]]>");
					builder.AppendLine("\t</data>");
				}

				if(_stackTrace != null && _stackTrace.Length > 0)
				{
					builder.AppendLine();
					builder.AppendLine("\t<stackTrace>");
					builder.AppendLine("\t<![CDATA[");
					builder.AppendLine(_stackTrace);
					builder.AppendLine("\t]]>");
					builder.AppendLine("\t</stackTrace>");
				}

				builder.AppendLine("</log>");

				_toString = builder.ToString();
			}

			return _toString;
		}

		private void WriteException(StringBuilder builder, Exception exception)
		{
			if(builder == null || exception == null)
				return;

			builder.AppendLine();
			builder.AppendFormat("\t<exception type=\"{0}, {1}\">" + Environment.NewLine, _exception.GetType().FullName, _exception.GetType().GetAssembly().GetName().Name);

			if(!string.Equals(_message, exception.Message))
			{
				builder.AppendLine("\t\t<message><![CDATA[" + exception.Message + "]]></message>");
			}

			if(exception.StackTrace != null && exception.StackTrace.Length > 0)
			{
				builder.AppendLine("\t\t<stackTrace>");
				builder.AppendLine("\t\t<![CDATA[");
				builder.AppendLine(exception.StackTrace);
				builder.AppendLine("\t\t]]>");
				builder.AppendLine("\t\t</stackTrace>");
			}

			builder.AppendLine("\t</exception>");

			if(exception.InnerException != null && exception.InnerException != exception)
				WriteException(builder, exception.InnerException);
		}

		#endregion
	}
}
