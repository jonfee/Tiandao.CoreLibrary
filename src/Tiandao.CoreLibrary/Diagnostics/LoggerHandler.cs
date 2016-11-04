using System;
using System.Collections.Generic;

namespace Tiandao.Diagnostics
{
    public class LoggerHandler : Services.CommandBase
	{
		#region 私有字段

		private ILogger _logger;

		#endregion

		#region 公共属性

		public ILogger Logger
		{
			get
			{
				return _logger;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException();

				_logger = value;
			}
		}

		#endregion

		#region 构造方法

		public LoggerHandler(string name, ILogger logger = null, LoggerHandlerPredication predication = null) : base(name)
		{
			_logger = logger;

			this.Predication = predication ?? new LoggerHandlerPredication();
		}

		#endregion

		#region 重写方法

		protected override bool CanExecute(object parameter)
		{
			return parameter is LogEntry && this.Logger != null && base.CanExecute(parameter);
		}

		protected override object OnExecute(object parameter)
		{
			var logger = this.Logger;

			if(logger != null)
				logger.Log(parameter as LogEntry);

			return null;
		}

		#endregion

		#region 公共方法

		public void Handle(LogEntry entry)
		{
			this.Execute(entry);
		}

		#endregion
	}
}