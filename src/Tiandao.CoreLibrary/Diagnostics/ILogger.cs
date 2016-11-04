using System;
using System.Collections.Generic;

namespace Tiandao.Diagnostics
{
    public interface ILogger
    {
		void Log(LogEntry entry);
	}
}