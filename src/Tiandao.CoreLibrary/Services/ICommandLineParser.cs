using System;

namespace Tiandao.Services
{
	/// <summary>
	/// 提供命令行文本解析功能。
	/// </summary>
	public interface ICommandLineParser 
    {
		/// <summary>
		/// 将指定的命令行文本解析成命令行对象。
		/// </summary>
		/// <param name="commandText">指定的要解析的命令行文本。</param>
		/// <returns>返回解析的命令行对象，如果解析失败则返回空(null)。</returns>
		CommandLine Parse(string commandText);
	}
}