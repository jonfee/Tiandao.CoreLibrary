using System;
using System.Linq;
using System.Reflection;

namespace Tiandao.Diagnostics
{
    public class LoggerHandlerPredication : Services.IPredication<LogEntry>
	{
		#region 私有字段

		private string _source;
		private LogLevel? _minLevel;
		private LogLevel? _maxLevel;
		private Type _exceptionType;

		#endregion

		#region 公共属性

		public string Source
		{
			get
			{
				return _source;
			}
			set
			{
				_source = value;
			}
		}

		public Type ExceptionType
		{
			get
			{
				return _exceptionType;
			}
			set
			{
				_exceptionType = value;
			}
		}

		public LogLevel? MinLevel
		{
			get
			{
				return _minLevel;
			}
			set
			{
				_minLevel = value;
			}
		}

		public LogLevel? MaxLevel
		{
			get
			{
				return _maxLevel;
			}
			set
			{
				_maxLevel = value;
			}
		}

		#endregion

		#region 断言方法

		public bool Predicate(LogEntry entry)
		{
			if(entry == null)
				return false;

			if(!string.IsNullOrWhiteSpace(this.Source))
			{
				var matched = true;
				var source = this.Source.Trim();

				if(source[0] == '*' || source[source.Length - 1] == '*')
				{
					if(source[0] == '*')
					{
						if(source[source.Length - 1] == '*')
							matched = entry.Source.Contains(source.Trim('*'));
						else
							matched = entry.Source.EndsWith(source.Trim('*'));
					}
					else
					{
						matched = entry.Source.StartsWith(source.Trim('*'));
					}
				}
				else
				{
					matched &= string.Equals(entry.Source, source, StringComparison.OrdinalIgnoreCase);
				}

				if(!matched)
					return false;
			}

			if(this.MinLevel.HasValue)
			{
				if(entry.Level < this.MinLevel.Value)
					return false;
			}

			if(this.MaxLevel.HasValue)
			{
				if(entry.Level > this.MaxLevel.Value)
					return false;
			}

			if(this.ExceptionType != null)
			{
				if(entry.Exception == null)
					return false;

				bool result;

				if(entry.Exception.GetType() == typeof(AggregateException))
				{
					result = ((AggregateException)entry.Exception).InnerExceptions.Any(ex => this.ExceptionType.IsAssignableFrom(ex.GetType()));
				}
				else
				{
					result = this.ExceptionType.IsAssignableFrom(entry.Exception.GetType());
				}

				if(!result)
					return false;
			}

			return true;
		}

		bool Services.IPredication.Predicate(object parameter)
		{
			return this.Predicate(parameter as LogEntry);
		}

		#endregion
	}
}