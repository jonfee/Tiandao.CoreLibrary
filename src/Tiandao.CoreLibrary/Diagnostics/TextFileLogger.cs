using System;
using System.IO;

namespace Tiandao.Diagnostics
{
    public class TextFileLogger : FileLogger
	{
		#region 重写方法

		protected override void WriteLog(LogEntry entry, Stream stream)
		{
			//注意：此处不能关闭stream参数传入的流，该流由基类确保安全释放！
			using(var writer = new StreamWriter(stream, System.Text.Encoding.UTF8, 1024 * 16, true))
			{
				writer.WriteLine(entry.ToString());
			}
		}

		#endregion
	}
}