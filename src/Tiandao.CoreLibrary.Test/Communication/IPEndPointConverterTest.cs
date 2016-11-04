using System;
using System.Net;

using Xunit;

namespace Tiandao.Communication.Test
{
    public class IPEndPointConverterTest
    {
		[Fact]
	    public void ParseTest()
		{
			Assert.NotNull(IPEndPointConverter.Parse("http://192.168.1.100:8080"));
			Assert.Null(IPEndPointConverter.Parse("http://www.google.com:80"));
		}
    }
}
