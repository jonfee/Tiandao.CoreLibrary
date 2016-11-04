using System;
using Tiandao.Common;
using Xunit;

namespace Tiandao.Resources.Test
{
    public class ResourceUtilityTest
    {
		[Fact]
	    public void GetStringTest()
		{
			Assert.Equal("命令", ResourceUtility.GetString("${Command}", typeof(ResourceUtilityTest).GetAssembly()));
			Assert.Equal("命令", ResourceUtility.GetString("Command", typeof(ResourceUtilityTest).GetAssembly()));
			Assert.Equal("'{0}'是一个无效的命令选项。", ResourceUtility.GetString("${InvalidCommandOption}", typeof(ResourceUtility).GetAssembly()));
		}
	}
}