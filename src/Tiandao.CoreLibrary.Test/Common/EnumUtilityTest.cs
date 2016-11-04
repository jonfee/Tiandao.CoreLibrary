using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using Tiandao.Test;

namespace Tiandao.Common.Test
{
    public class EnumUtilityTest
	{
		[Fact]
	    public void GetEnumTypeTest()
		{
			Assert.Equal(typeof(Gender), EnumUtility.GetEnumType(typeof(Gender?)));
			Assert.Equal(typeof(Gender), EnumUtility.GetEnumType(typeof(Nullable<Gender>)));
			Assert.Equal(typeof(Gender), EnumUtility.GetEnumType(Gender.Male.GetType()));
		}

	    [Fact]
	    public void FormatTest()
		{
			Assert.Equal("先生", EnumUtility.Format(Gender.Male, "d"));
			Assert.Equal("Male", EnumUtility.Format(Gender.Male, "n"));
			Assert.Equal("M", EnumUtility.Format(Gender.Male, "a"));
		}

	    [Fact]
	    public void GetEnumEntryTest()
	    {
			var entry = EnumUtility.GetEnumEntry(Gender.Female);
			Assert.Equal("Female", entry.Name);
			Assert.Equal(Gender.Female, entry.Value); //注意：entry.Value 为枚举类型
			Assert.Equal("F", entry.Alias);
			Assert.Equal("女士", entry.Description);

			entry = EnumUtility.GetEnumEntry(Gender.Male, true); //注意：underlyingType 参数值为 true
			Assert.Equal("Male", entry.Name);
			Assert.Equal(0, entry.Value); //注意：entry.Value 为枚举项的基元类型
			Assert.Equal("M", entry.Alias);
			Assert.Equal("先生", entry.Description);
		}

		[Fact]
	    public void GetEnumEntriesTest()
	    {
			var entries = EnumUtility.GetEnumEntries(typeof(Gender), true);
			Assert.Equal(2, entries.Length);
			Assert.Equal("Male", entries[0].Name);
			Assert.Equal("Female", entries[1].Name);

			entries = EnumUtility.GetEnumEntries(typeof(Nullable<Gender>), true, -1, "<Unknown>");
			Assert.Equal(3, entries.Length);
			Assert.Equal("", entries[0].Name);
			Assert.Equal(-1, entries[0].Value);
			Assert.Equal("<Unknown>", entries[0].Description);

			Assert.Equal("Male", entries[1].Name);
			Assert.Equal("Female", entries[2].Name);
		}
	}
}
