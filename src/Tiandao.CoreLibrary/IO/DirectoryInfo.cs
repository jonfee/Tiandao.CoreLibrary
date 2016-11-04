using System;
using System.Collections.Generic;

namespace Tiandao.IO
{
#if !CORE_CLR
	[Serializable]
#endif
	public class DirectoryInfo : PathInfo
	{
		#region 重写属性

		public override bool IsFile
		{
			get
			{
				return false;
			}
		}

		public override bool IsDirectory
		{
			get
			{
				return true;
			}
		}

		#endregion

		#region 构造方法

		protected DirectoryInfo()
		{

		}

		public DirectoryInfo(string path, DateTime? createdTime = null, DateTime? modifiedTime = null, string url = null) : base(path, createdTime, modifiedTime, url)
		{

		}

		public DirectoryInfo(Path path, DateTime? createdTime = null, DateTime? modifiedTime = null, string url = null) : base(path, createdTime, modifiedTime, url)
		{

		}

		#endregion
	}
}