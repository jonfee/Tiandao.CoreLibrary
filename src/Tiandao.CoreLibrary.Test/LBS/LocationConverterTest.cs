using System;
using System.Collections.Generic;

using Xunit;

namespace Tiandao.LBS.Test
{
	public class LocationConverterTest
	{
		/// <summary>
		/// 可通过：http://www.gpsspg.com/maps.htm 验证转换结果。
		/// </summary>
		[Fact]
	    public void ConvertValueTest()
		{
			//硬件坐标(84)转百度坐标(09) 结果：22.497601820474,113.922039598097
			Assert.NotNull(LocationConverter.Convert84To09(22.4948556106, 113.9106398730));

			//硬件坐标(84)转高德/腾讯坐标(02) 结果：22.4918022270248,113.915497680362
			Assert.NotNull(LocationConverter.Convert84To02(22.4948556106, 113.9106398730));

			//高德腾讯坐标(02)转百度坐标(09) 结果：22.4976512346523,113.922090103635
			Assert.NotNull(LocationConverter.Convert02To09(22.4918523806, 113.9155480930));

			//高德腾讯坐标(02)转硬件坐标(84) 结果：22.4949033119611,113.9106913112
			Assert.NotNull(LocationConverter.Convert02To84(22.4918523806, 113.9155480930));

			//百度坐标(09)转腾讯高德坐标(02) 结果：22.4918188289114,113.915495518286
			Assert.NotNull(LocationConverter.Convert09To02(22.4977230000, 113.9220230000));

			//百度坐标(09)转硬件坐标(84) 结果：22.4948697923719,113.910638735789
			Assert.NotNull(LocationConverter.Convert09To84(22.4977230000, 113.9220230000));
		}
	}
}