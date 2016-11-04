using System;
using System.Security.Cryptography;
using System.Text;

namespace Tiandao.Common
{
	/// <summary>
	/// 随机数生成器类。
	/// </summary>
	public static class RandomGenerator
	{
		#region 常量定义

		private const string DIGITS = "0123456789ABCDEFGHJKMNPRSTUVWXYZ";

		#endregion

		#region 私有字段

		private static readonly RandomNumberGenerator _generator;

		#endregion

		#region 构造方法

		static RandomGenerator()
		{
			_generator = RandomNumberGenerator.Create();
		}

		#endregion

		#region 公共方法

		public static byte[] Generate(int length)
		{
			if(length < 1)
				throw new ArgumentOutOfRangeException("length");

			var bytes = new byte[length];

			_generator.GetBytes(bytes);

			return bytes;
		}

		public static int GenerateInt32()
		{
			var bytes = new byte[4];

			_generator.GetBytes(bytes);

			return BitConverter.ToInt32(bytes, 0);
		}

		public static long GenerateInt64()
		{
			var bytes = new byte[8];

			_generator.GetBytes(bytes);

			return BitConverter.ToInt64(bytes, 0);
		}

		public static string GenerateString()
		{
			return GenerateString(8);
		}

		public static string GenerateString(int length, bool digitOnly = false)
		{
			if(length < 1 || length > 128)
				throw new ArgumentOutOfRangeException("length");

			var result = new char[length];
			var data = new byte[length];

			_generator.GetBytes(data);

			//确保首位字符始终为数字字符
			result[0] = DIGITS[data[0] % 10];

			for(int i = 1; i < length; i++)
			{
				result[i] = DIGITS[data[i] % (digitOnly ? 10 : 32)];
			}

			return new string(result);
		}

		/// <summary>
		/// 生成一个指定长度的随机整数字符串。
		/// </summary>
		/// <param name="length">长度。</param>
		/// <returns>返回指定长度的随机字符串。</returns>
		public static string GenerateNumber(int length)
		{
			var result = new StringBuilder(length);

			while(result.Length < length)
			{
				result.Append(BitConverter.ToUInt64(GenerateSalt(12), 0));
			}

			if(result.Length > length)
			{
				result = result.Remove(length - 1, result.Length - length);
			}

			return result.ToString();
		}

		/// <summary>
		/// 生成一个指定长度的随机混合(字母和数字)字符串。
		/// </summary>
		/// <param name="length">长度。</param>
		/// <returns>返回指定长度的随机字符串。</returns>
		public static string GenerateMixinCode(int length)
		{
			var result = new StringBuilder(length);

			while(result.Length < length)
			{
				result.Append(BitConverter.ToString(GenerateSalt(12), 0).Replace("-", string.Empty));
			}

			if(result.Length > length)
			{
				result = result.Remove(length - 1, result.Length - length);
			}

			return result.ToString();
		}

		/// <summary>
		/// 生成一个指定长度的随机中文字符串。
		/// </summary>
		/// <param name="length">长度。</param>
		/// <returns>返回指定长度的随机字符串。</returns>
		public static string GenerateChinese(int length)
		{
			// 获取GB2312编码页
			var encoding = Encoding.GetEncoding("gb2312");
			var bytes = GenerateRegionCode(length);
			var builder = new StringBuilder();

			for(int i = 0; i < length; i++)
			{
				builder.Append(encoding.GetString((byte[])Convert.ChangeType(bytes[i], typeof(byte[]))));
			}

			return builder.ToString();
		}

		#endregion

		#region 私有方法

		/// <summary>
		/// 此函数在汉字编码范围内随机创建含两个元素的十六进制字节数组，每个字节数组代表一个汉字。
		/// </summary>
		/// <param name="length">指定要生成的汉字个数。</param>
		/// <returns>返回生成指定长度的汉字字节数组。</returns>
		private static object[] GenerateRegionCode(int length)
		{
			//定义一个字符串数组储存汉字编码的组成元素 
			string[] chars = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

			Random random = new Random();

			//定义一个object数组。
			object[] bytes = new object[length];

			/*
			 每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bytes数组中 
			 每个汉字有四个区位码组成 
			 区位码第1位和区位码第2位作为字节数组第一个元素 
			 区位码第3位和区位码第4位作为字节数组第二个元素 
			*/
			for(int i = 0; i < length; i++)
			{
				//区位码第1位 
				int num1 = random.Next(11, 14);

				string str1 = chars[num1].Trim();

				//区位码第2位 
				random = new Random(num1 * unchecked((int)DateTime.Now.Ticks) + i); // 更换随机数发生器的种子避免产生重复值

				int num2 = random.Next(0, num1 == 13 ? 7 : 16);

				string str2 = chars[num2].Trim();

				//区位码第3位 
				random = new Random(num2 * unchecked((int)DateTime.Now.Ticks) + i);

				int num3 = random.Next(10, 16);

				string str3 = chars[num3].Trim();

				//区位码第4位 
				random = new Random(num3 * unchecked((int)DateTime.Now.Ticks) + i);

				int num4;

				if(num3 == 10)
				{
					num4 = random.Next(1, 16);
				}
				else if(num3 == 15)
				{
					num4 = random.Next(0, 15);
				}
				else
				{
					num4 = random.Next(0, 16);
				}

				string str4 = chars[num4].Trim();

				// 定义两个字节变量存储产生的随机汉字区位码 
				byte byte1 = Convert.ToByte(str1 + str2, 16);
				byte byte2 = Convert.ToByte(str3 + str4, 16);

				// 将两个字节变量存储在字节数组中 
				byte[] buffer = new byte[] { byte1, byte2 };

				// 将产生的一个汉字的字节数组放入object数组中 
				bytes.SetValue(buffer, i);
			}

			return bytes;
		}

		/// <summary>
		/// 生成随机的Salt值。
		/// </summary>
		/// <param name="length">指定要生成的Salt字节数组的长度，如果长度小于4则为4。</param>
		/// <returns>返回生成字节数组的Salt值。</returns>
		private static byte[] GenerateSalt(int length)
		{
			byte[] salt = new byte[Math.Max(length, 4)];

			_generator.GetBytes(salt);

			return salt;
		}

		#endregion
	}
}