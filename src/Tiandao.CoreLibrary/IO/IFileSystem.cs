using System;
using System.Collections.Generic;

namespace Tiandao.IO
{
	/// <summary>
	/// 表示文件目录系统的接口。
	/// </summary>
	public interface IFileSystem
    {
		/// <summary>
		/// 获取文件目录系统的方案名。
		/// </summary>
		string Scheme
		{
			get;
		}

		/// <summary>
		/// 获取文件操作服务。
		/// </summary>
		IFile File
		{
			get;
		}

		/// <summary>
		/// 获取目录操作服务。
		/// </summary>
		IDirectory Directory
		{
			get;
		}

		/// <summary>
		/// 获取本地路径对应的外部访问Url地址。
		/// </summary>
		/// <param name="path">要获取的本地路径。</param>
		/// <returns>返回对应的Url地址。</returns>
		/// <remarks>
		///		<para>本地路径：是指特定的<see cref="IFileSystem"/>文件目录系统的路径格式。</para>
		///		<para>外部访问Url地址：是指可通过Web方式访问某个文件或目录的URL。</para>
		/// </remarks>
		string GetUrl(string path);
	}
}
