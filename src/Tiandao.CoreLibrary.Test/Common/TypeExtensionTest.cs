using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Xunit;
using Tiandao.Test;
using Tiandao.ComponentModel;

namespace Tiandao.Common.Test
{
    public class TypeExtensionTest
    {
		[Fact]
	    public void AssignableTest()
		{
			Assert.True(TypeExtension.IsAssignableFrom(typeof(IEnumerable), typeof(IDictionary<string, object>)));
			Assert.True(TypeExtension.IsAssignableFrom(typeof(IEnumerable), typeof(IList)));
			Assert.True(TypeExtension.IsAssignableFrom(typeof(IEnumerable), typeof(int[])));
			Assert.True(TypeExtension.IsAssignableFrom(typeof(IList), typeof(List<string>)));
		}

		[Fact]
	    public void ScalarTypeTest()
	    {
			Assert.True(TypeExtension.IsScalarType(typeof(string)));
			Assert.True(TypeExtension.IsScalarType(typeof(Guid)));
			Assert.True(TypeExtension.IsScalarType(typeof(DateTime)));
			Assert.True(TypeExtension.IsScalarType(typeof(TimeSpan)));
			Assert.True(TypeExtension.IsScalarType(typeof(Gender)));
			Assert.True(TypeExtension.IsScalarType(typeof(int[])));

			Assert.False(TypeExtension.IsScalarType(typeof(IList<int>)));
		}

		[Fact]
	    public void GetTypeTest()
	    {
			Assert.Equal(typeof(string), TypeExtension.GetType("string"));
			Assert.Equal(typeof(DateTime?), TypeExtension.GetType("datetime?"));
			Assert.Equal(typeof(System.Text.StringBuilder), TypeExtension.GetType("System.Text.StringBuilder"));
			Assert.Equal(typeof(EnumEntry), TypeExtension.GetType("Tiandao.Common.EnumEntry"));
		}

		[Fact]
	    public void GetTypeCodeTest()
		{
			string var1 = "";
			int var2 = 0;

			Assert.Equal(TypeCode.String, var1.GetType().GetTypeCode());
			Assert.Equal(TypeCode.Int32, var2.GetType().GetTypeCode());
		}

		[Fact]
		public void GetCustomAttributesTest()
		{
			Assert.Equal(2, typeof(Address).GetCustomAttributes().Count());
			Assert.Equal(1, typeof(Address).GetCustomAttributes(typeof(DisplayNameAttribute), true).Count());
		}
	}
}
