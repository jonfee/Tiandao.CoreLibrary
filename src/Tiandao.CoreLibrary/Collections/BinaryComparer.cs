using System;
using System.Collections;
using System.Collections.Generic;

namespace Tiandao.Collections
{
	/// <summary>
	/// 定义方法以支持二进制数据的相等比较。
	/// </summary>
	public class BinaryComparer : IEqualityComparer, IEqualityComparer<byte[]>
	{
		#region 静态实例

		public static readonly BinaryComparer Default = new BinaryComparer();

		#endregion

		#region 公共方法

		public bool Equals(byte[] x, byte[] y)
		{
			if(x == null && y == null)
				return true;

			if(x == null || y == null || x.Length != y.Length)
				return false;

			for(int i = 0; i < x.Length; i++)
			{
				if(x[i] != y[i])
					return false;
			}

			return true;
		}

		public int GetHashCode(byte[] obj)
		{
			if(obj == null || obj.Length == 0)
				return 0;

			if(obj.Length == 4)
				return BitConverter.ToInt32(obj, 0);

			int result = 0;

			for(int i = 0; i < obj.Length; i++)
			{
				result ^= obj[i] << (i % 4) * 8;
			}

			return result;
		}

		public new bool Equals(object x, object y)
		{
			if(x == null && y == null)
				return true;

			if(x == null || y == null || x.GetType() != typeof(byte[]) || y.GetType() != typeof(byte[]))
				return false;

			return this.Equals((byte[])x, (byte[])y);
		}

		public int GetHashCode(object obj)
		{
			if(obj == null || obj.GetType() != typeof(byte[]))
				return 0;

			return this.GetHashCode((byte[])obj);
		}

		#endregion
	}
}
