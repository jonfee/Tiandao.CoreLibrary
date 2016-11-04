﻿using System;
using System.Collections.Generic;

namespace Tiandao.IO
{
#if !CORE_CLR
	[Serializable]
#endif
	public class PathInfo
    {
		#region 私有字段

		private Path _path;
		private string _url;
		private DateTime _createdTime;
		private DateTime _modifiedTime;
		private Dictionary<string, string> _properties;

		#endregion

		#region 公共属性

		/// <summary>
		/// 获取路径信息的名称。
		/// </summary>
		/// <remarks>
		///		<para>该属性不同于<see cref="Path"/>属性指向的<seealso cref="System.IO.Path"/>路径对象中的<seealso cref="System.IO.Path.FileName"/>或<seealso cref="System.IO.Path.DirectoryName"/>属性值。</para>
		///		<para>如果<see cref="IsFile"/>属性为真(True)，则返回<see cref="Path"/>属性指向的<seealso cref="System.IO.Path.FileName"/>属性值。</para>
		///		<para>如果<see cref="IsDirectory"/>属性为真(True)，则返回<see cref="Path"/>属性指向的<seealso cref="System.IO.Path.DirectoryName"/>属性值表示的目录路径中的最后一个子目录的名称。</para>
		/// </remarks>
		public string Name
		{
			get
			{
				var path = _path;

				if(path == null)
					return null;

				if(path.IsFile)
					return path.FileName;

				if(path.IsDirectory)
				{
					var directoryName = path.DirectoryName;

					if(directoryName != null && directoryName.Length > 1)
					{
						int offset = directoryName.EndsWith("/") ? directoryName.Length - 2 : directoryName.Length - 1;
						int index = directoryName.LastIndexOf('/', offset);

						if(index >= 0)
							return directoryName.Substring(index + 1, offset - index) + "/";
					}

					return directoryName;
				}

				return string.Empty;
			}
		}

		public Path Path
		{
			get
			{
				return _path;
			}
			protected set
			{
				if(value == null)
					throw new ArgumentNullException();

				_path = value;
			}
		}

		/// <summary>
		/// 获取或设置外部访问的URL地址。
		/// </summary>
		/// <remarks>有关外部访问的URL请参考：<seealso cref="IFileSystem.GetUrl"/>方法。</remarks>
		public virtual string Url
		{
			get
			{
				if(_url != null)
					return _url;

				var path = _path;

				if(path == null)
					return string.Empty;

				return path.Url;
			}
			set
			{
				_url = value;
			}
		}

		public virtual bool IsFile
		{
			get
			{
				var path = this.Path;

				if(path == null)
					return false;

				return path.IsFile;
			}
		}

		public virtual bool IsDirectory
		{
			get
			{
				var path = this.Path;

				if(path == null)
					return false;

				return path.IsDirectory;
			}
		}

		public DateTime CreatedTime
		{
			get
			{
				return _createdTime;
			}
			protected set
			{
				_createdTime = value;
			}
		}

		public DateTime ModifiedTime
		{
			get
			{
				return _modifiedTime;
			}
			protected set
			{
				_modifiedTime = value;
			}
		}

		public bool HasProperties
		{
			get
			{
				return _properties != null && _properties.Count > 0;
			}
		}

		public IDictionary<string, string> Properties
		{
			get
			{
				if(_properties == null)
					System.Threading.Interlocked.CompareExchange(ref _properties, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase), null);

				return _properties;
			}
		}

		#endregion

		#region 构造方法

		protected PathInfo()
		{
		}

		public PathInfo(string path, DateTime? createdTime = null, DateTime? modifiedTime = null, string url = null)
		{
			if(string.IsNullOrWhiteSpace(path))
				throw new ArgumentNullException("path");

			_path = Path.Parse(path);
			_url = url;

			if(createdTime.HasValue)
				_createdTime = createdTime.Value;

			if(modifiedTime.HasValue)
				_modifiedTime = modifiedTime.Value;
			else
				_modifiedTime = _createdTime;
		}

		public PathInfo(Path path, DateTime? createdTime = null, DateTime? modifiedTime = null, string url = null)
		{
			if(path == null)
				throw new ArgumentNullException("path");

			_path = path;
			_url = url;

			if(createdTime.HasValue)
				_createdTime = createdTime.Value;

			if(modifiedTime.HasValue)
				_modifiedTime = modifiedTime.Value;
			else
				_modifiedTime = _createdTime;
		}

		#endregion

		#region 重写方法

		public override string ToString()
		{
			if(_path == null)
				return base.ToString();

			return _path.ToString();
		}

		public override int GetHashCode()
		{
			if(_path == null)
				return base.GetHashCode();

			return _path.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if(obj == null || obj.GetType() != this.GetType())
				return false;

			if(_path == null)
				return base.Equals(obj);

			return _path.Equals(((PathInfo)obj).Path);
		}

		#endregion
	}
}