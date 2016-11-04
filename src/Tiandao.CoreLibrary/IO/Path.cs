﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Tiandao.IO
{
	/// <summary>
	/// 表示不依赖操作系统的路径。
	/// </summary>
	/// <remarks>
	///		<para>路径格式分为<seealso cref="Path.Scheme"/>和<seealso cref="Path.FullPath"/>这两个部分，中间使用冒号(:)分隔，路径各层级间使用正斜杠(/)进行分隔。如果是目录路径则以正斜杠(/)结尾。</para>
	///		<para>其中<seealso cref="Path.Scheme"/>可以省略，如果为目录路径，则<see cref="Path.FileName"/>属性为空或空字符串("")。常用路径示例如下：</para>
	///		<list type="bullet">
	///			<item>
	///				<term>某个文件的<see cref="Url"/>：zfs:/data/attachments/2014/07/file-name.ext</term>
	///			</item>
	///			<item>
	///				<term>某个本地文件的<see cref="Url"/>：zfs.local:/data/attachments/2014/07/file-name.ext</term>
	///			</item>
	///			<item>
	///				<term>某个分布式文件的<see cref="Url"/>：zfs.distributed:/data/attachments/file-name.ext</term>
	///			</item>
	///			<item>
	///				<term>某个目录的<see cref="Url"/>：zfs:/data/attachments/2014/07/</term>
	///			</item>
	///			<item>
	///				<term>未指定模式(Scheme)的<see cref="Url"/>：/data/attachements/images/</term>
	///			</item>
	///		</list>
	/// </remarks>
	public sealed class Path
    {
		#region 常量定义

		private const string SCHEME_REGEX = @"\s*((?<scheme>[A-Za-z]+(\.[A-Za-z_\-]+)?):)?";
		private const string PATH_REGEX = @"(?<path>(?<part>[/\\][^/\\\*\?:]+)*(?<part>[/\\])?)\s*";

		#endregion

		#region 私有变量

		private static readonly Regex _regex = new Regex("^" + SCHEME_REGEX + PATH_REGEX + "$", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
		
		#endregion

		#region 成员字段

		private string _originalString;
		private string _scheme;
		private string _fullPath;
		private string _directoryName;
		private string _fileName;

		#endregion

		#region 构造函数

		private Path(string originalString, string scheme, string fullPath)
		{
			if(string.IsNullOrWhiteSpace(originalString))
				throw new ArgumentNullException("originalString");

			_originalString = originalString.Trim();
			_scheme = string.IsNullOrWhiteSpace(scheme) ? null : scheme.Trim().ToLowerInvariant();

			if(string.IsNullOrWhiteSpace(fullPath))
			{
				_fullPath = "/";
				_directoryName = "/";
				_fileName = string.Empty;

				return;
			}

			_fullPath = fullPath.Trim();

			var parts = _fullPath.Split('/', '\\');
			_fileName = parts[parts.Length - 1];
			_directoryName = string.Join("/", parts, 0, parts.Length - 1) + "/";
		}

		#endregion

		#region 公共属性

		public string Scheme
		{
			get
			{
				return _scheme;
			}
		}

		public string DirectoryName
		{
			get
			{
				return _directoryName;
			}
		}

		public string FileName
		{
			get
			{
				return _fileName;
			}
		}

		public string FullPath
		{
			get
			{
				return _fullPath;
			}
		}

		public string Url
		{
			get
			{
				return string.IsNullOrEmpty(_scheme) ? _fullPath : (_scheme + ":" + _fullPath);
			}
		}

		public bool IsFile
		{
			get
			{
				return !string.IsNullOrEmpty(_fileName);
			}
		}

		public bool IsDirectory
		{
			get
			{
				return string.IsNullOrEmpty(_fileName);
			}
		}

		#endregion

		#region 重写方法

		public override string ToString()
		{
			if(string.IsNullOrWhiteSpace(_scheme))
				return _fullPath;
			else
				return _scheme + ":" + _fullPath;
		}

		public override int GetHashCode()
		{
#if !CORE_CLR
			switch(Environment.OSVersion.Platform)
			{
				case PlatformID.MacOSX:
				case PlatformID.Unix:
					return (_scheme + _fullPath).GetHashCode();
				default:
					return (_scheme + _fullPath.ToLowerInvariant()).GetHashCode();
			}
#else
			if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				return (_scheme + _fullPath).GetHashCode();
			}
			else
			{
				return (_scheme + _fullPath.ToLowerInvariant()).GetHashCode();
			}
#endif
		}

		public override bool Equals(object obj)
		{
			if(obj == null || obj.GetType() != this.GetType())
				return false;

			var other = (Path)obj;

			if(!string.Equals(_scheme, other._scheme))
				return false;

#if !CORE_CLR
			switch(Environment.OSVersion.Platform)
			{
				case PlatformID.MacOSX:
				case PlatformID.Unix:
					return string.Equals(_fullPath, other._fullPath, StringComparison.Ordinal);
				default:
					return string.Equals(_fullPath, other._fullPath, StringComparison.OrdinalIgnoreCase);
			}
#else
			if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				return string.Equals(_fullPath, other._fullPath, StringComparison.Ordinal);
			}
			else
			{
				return string.Equals(_fullPath, other._fullPath, StringComparison.OrdinalIgnoreCase);
			}
#endif

		}

		#endregion

		#region 静态方法

		/// <summary>
		/// 解析文本格式的路径。
		/// </summary>
		/// <param name="text">要解析的路径文本。</param>
		/// <returns>返回解析成功的<see cref="Path"/>路径对象。</returns>
		/// <exception cref="ArgumentNullException">当<paramref name="text"/>参数为空或空白字符串。</exception>
		/// <exception cref="PathException">当<paramref name="text"/>参数为无效的路径格式。</exception>
		public static Path Parse(string text)
		{
			if(string.IsNullOrWhiteSpace(text))
				throw new ArgumentNullException("text");

			string scheme, path;

			if(TryParse(text, out scheme, out path))
				return new Path(text, scheme, path);

			throw new PathException(text);
		}

		/// <summary>
		/// 尝试解析文本格式的路径。
		/// </summary>
		/// <param name="text">要解析的路径文本。</param>
		/// <param name="result">解析成功的<see cref="Path"/>路径对象。</param>
		/// <returns>如果解析成功则返回真(True)，否则返回假(False)。</returns>
		public static bool TryParse(string text, out Path result)
		{
			result = null;
			string scheme, path;

			if(TryParse(text, out scheme, out path))
			{
				result = new Path(text, scheme, path);
				return true;
			}

			return false;
		}

		public static bool TryParse(string text, out string scheme, out string path)
		{
			scheme = null;
			path = null;

			if(string.IsNullOrWhiteSpace(text))
				return false;

			var match = _regex.Match(text);

			if(match.Success)
			{
				scheme = string.IsNullOrWhiteSpace(match.Groups["scheme"].Value) ? null : match.Groups["scheme"].Value;
				path = match.Groups["path"].Value.Replace('\\', '/');
			}

			return match.Success;
		}

		/// <summary>
		/// 解析路径文本中指定的方案名。
		/// </summary>
		/// <param name="text">指定的路径文本。</param>
		/// <returns>返回的路径方案名，如果解析失败或者路径文本未包含方案则返回空。</returns>
		public static string GetScheme(string text)
		{
			if(string.IsNullOrWhiteSpace(text))
				return null;

			var match = _regex.Match(text);

			if(match.Success)
				return string.IsNullOrWhiteSpace(match.Groups["scheme"].Value) ? null : match.Groups["scheme"].Value;

			return null;
		}

		public static string GetPath(string text)
		{
			if(string.IsNullOrWhiteSpace(text))
				return null;

			var match = _regex.Match(text);

			if(match.Success)
				return match.Groups["path"].Value.Replace('\\', '/');

			return null;
		}

		public static string Combine(params string[] parts)
		{
			if(parts == null || parts.Length == 0)
				throw new ArgumentNullException("parts");

			string result = string.Empty;

			for(int i = 0; i < parts.Length; i++)
			{
				if(string.IsNullOrWhiteSpace(parts[i]))
					continue;

				var part = parts[i].Replace('\\', '/').Trim();

				if(result.Length == 0)
					result = part;
				else
				{
					if(result.EndsWith("/"))
						result += part.TrimStart('/');
					else
						result += "/" + part.TrimStart('/');
				}
			}

			return result;
		}

		#endregion
	}
}
