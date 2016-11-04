using System;
using System.Collections.Generic;

namespace Tiandao.Diagnostics
{
    public class ConsoleLogger : ILogger
	{
#if !CORE_CLR
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
#endif
		public void Log(LogEntry entry)
	    {
			if(entry == null)
				return;

			//根据日志级别来调整控制台的前景色
			switch(entry.Level)
			{
				case LogLevel.Trace:
					Console.ForegroundColor = ConsoleColor.Gray;
					break;
				case LogLevel.Debug:
					Console.ForegroundColor = ConsoleColor.DarkGray;
					break;
				case LogLevel.Warn:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case LogLevel.Error:
				case LogLevel.Fatal:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
			}

			try
			{
				//打印日志信息
				Console.WriteLine(entry);
			}
			finally
			{
				//恢复默认颜色
				Console.ResetColor();
			}
		}
	}
}