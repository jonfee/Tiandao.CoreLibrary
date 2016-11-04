using System;
using System.Collections.Generic;

using Xunit;

namespace Tiandao.Common.Test
{
    public class StringExtensionTest
    {
		[Fact]
		public void ContainsCharactersTest()
		{
			Assert.True(StringExtension.ContainsCharacters("abcdefghijk", "big"));
			Assert.True(StringExtension.ContainsCharacters("abcdefghijk", "biger"));
			Assert.False(StringExtension.ContainsCharacters("abcdefghijk", "xyz"));
		}

		[Fact]
		public void RemoveCharactersTest()
		{
			const string TEXT = @"Read^me??.txt";

			Assert.Equal("Read^me.txt", StringExtension.RemoveCharacters(TEXT, "?"));
			Assert.Equal("Readme.txt", StringExtension.RemoveCharacters(TEXT, "?^"));
		}

		[Fact]
		public void TrimStringTest()
		{
			Assert.Equal("ContentSuffix", StringExtension.TrimString("PrefixPrefixContentSuffix", "Prefix"));
			Assert.Equal("PrefixPrefixContent", StringExtension.TrimString("PrefixPrefixContentSuffix", "Suffix"));
			Assert.Equal("Content", StringExtension.TrimString("PrefixPrefixContentSuffix", "Prefix", "Suffix"));
		}
	}
}
