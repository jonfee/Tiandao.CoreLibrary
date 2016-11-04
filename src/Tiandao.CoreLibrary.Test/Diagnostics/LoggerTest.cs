using System;
using System.Collections.Generic;

using Tiandao.ComponentModel;

using Xunit;

namespace Tiandao.Diagnostics.Test
{
	public class ApplicationContext : ApplicationContextBase
	{
		#region 私有构造

		public ApplicationContext() : base("Tiandao.Diagnostics.Tests")
		{
			ApplicationContextBase.Current = this;
		}

		#endregion
	}

	public class LoggerTest
	{
		[Fact]
		public void WriteLog()
		{
			var app = new ApplicationContext();

			var clr = this.GetCLR();

			var handler = new LoggerHandler
			(
				"File", 
				new TextFileLogger
				{
					FilePath = "logs/${binding:timestamp#yyyyMM}/${binding:source}[{sequence}].log"
				},
				new LoggerHandlerPredication()
				{
					MinLevel = LogLevel.Info
				}
			);

			Logger.Handlers.Add(handler);

			Logger.Info("Hello Word!");
		}

		private string GetCLR()
		{
#if DNX451
			return "DNX451:";
#endif

			return "DNX50:";
		}
	}
}