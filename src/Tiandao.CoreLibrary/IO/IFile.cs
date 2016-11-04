using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tiandao.IO
{
	/// <summary>
	/// 提供用于创建、复制、删除、移动和打开文件等功能的抽象接口，该接口将提供不同文件系统的文件支持。
	/// </summary>
	public interface IFile
    {
		/// <summary>
		/// 获取指定文件路径对应的<see cref="FileInfo"/>描述信息。
		/// </summary>
		/// <param name="path">指定的文件路径。</param>
		/// <returns>如果指定的路径是存在的则返回对应的<see cref="FileInfo"/>，否则返回空(null)。</returns>
		FileInfo GetInfo(string path);
		Task<FileInfo> GetInfoAsync(string path);

		bool SetInfo(string path, IDictionary<string, string> properties);
		Task<bool> SetInfoAsync(string path, IDictionary<string, string> properties);

		bool Delete(string path);
		Task<bool> DeleteAsync(string path);

		bool Exists(string path);
		Task<bool> ExistsAsync(string path);

		void Copy(string source, string destination);
		void Copy(string source, string destination, bool overwrite);

		Task CopyAsync(string source, string destination);
		Task CopyAsync(string source, string destination, bool overwrite);

		void Move(string source, string destination);
		Task MoveAsync(string source, string destination);

		Stream Open(string path, IDictionary<string, string> properties = null);
		Stream Open(string path, FileMode mode, IDictionary<string, string> properties = null);
		Stream Open(string path, FileMode mode, FileAccess access, IDictionary<string, string> properties = null);
		Stream Open(string path, FileMode mode, FileAccess access, FileShare share, IDictionary<string, string> properties = null);
	}
}
