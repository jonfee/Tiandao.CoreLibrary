using System;
using System.Collections.Generic;

namespace Tiandao.IO
{
#if !CORE_CLR
	[Serializable]
#endif
	public class FileInfo : PathInfo
    {
		#region 私有字段

		private long _size;
		private byte[] _checksum;

		#endregion

		#region 公共属性

		public byte[] Checksum
		{
			get
			{
				return _checksum;
			}
			set
			{
				_checksum = value;
			}
		}

		public long Size
		{
			get
			{
				return _size;
			}
			set
			{
				_size = value;
			}
		}

		public override bool IsFile
		{
			get
			{
				return true;
			}
		}

		public override bool IsDirectory
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region 构造方法

		protected FileInfo()
		{
		}

		public FileInfo(string path, long size, DateTime? createdTime = null, DateTime? modifiedTime = null, string url = null) : base(path, createdTime, modifiedTime, url)
		{
			_size = size;
		}

		public FileInfo(Path path, long size, DateTime? createdTime = null, DateTime? modifiedTime = null, string url = null) : base(path, createdTime, modifiedTime, url)
		{
			_size = size;
		}

		public FileInfo(string path, long size, byte[] checksum, DateTime? createdTime = null, DateTime? modifiedTime = null, string url = null)
			: base(path, createdTime, modifiedTime, url)
		{
			_size = size;
			_checksum = checksum;
		}

		public FileInfo(Path path, long size, byte[] checksum, DateTime? createdTime = null, DateTime? modifiedTime = null, string url = null)
			: base(path, createdTime, modifiedTime, url)
		{
			_size = size;
			_checksum = checksum;
		}

		#endregion

		#region 重写方法

		public override int GetHashCode()
		{
			var path = this.Path;
			var text = _size.ToString() + Common.Converter.ToHexString(_checksum);

			if(path == null)
				return text.GetHashCode();
			else
				return (path.Url + text).GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if(obj == null || obj.GetType() != this.GetType())
				return false;

			var other = (FileInfo)obj;

			if(_size != other._size || !Collections.BinaryComparer.Default.Equals(_checksum, other._checksum))
				return false;

			var path = this.Path;

			return path == null ? true : path.Equals(other.Path);
		}

		#endregion
	}
}
