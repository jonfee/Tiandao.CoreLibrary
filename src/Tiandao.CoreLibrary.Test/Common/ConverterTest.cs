using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using Tiandao.Test;
using Tiandao.Collections;

namespace Tiandao.Common.Test
{
	public class ConverterTest
	{
		[Fact]
		public void ConvertValueTest()
		{
			Assert.Equal(true, "1".ToBoolean());
			Assert.Equal(false, "0".ToBoolean());
			Assert.Equal(true, "True".ToBoolean());
			Assert.Equal(false, "False".ToBoolean());
			Assert.Equal(true, "TRUE".ToBoolean());
			Assert.Equal(false, "FALSE".ToBoolean());
			Assert.Equal(true, Converter.ConvertValue<bool>("true"));
			Assert.Equal(false, Converter.ConvertValue<bool>("false"));

			Assert.Equal(new DateTime(2016, 4, 5, 19, 9, 41), "2016-04-05 19:09:41".ToDateTime());
			Assert.Equal(new DateTime(2016, 4, 5), Converter.ConvertValue<DateTime>("2016-04-05"));

			Assert.Equal(123, Converter.ConvertValue<int>("123"));
			Assert.Equal(-1, Converter.ConvertValue<int>("aa123bb", -1));

			Assert.Null(Converter.ConvertValue<int?>("", () => null));
			Assert.Null(Converter.ConvertValue<int?>("x", () => null));
			Assert.NotNull(Converter.ConvertValue<int?>("123", () => null));
			Assert.Equal(123, Converter.ConvertValue<int?>("123", () => null));
			
			Assert.Equal("100", Converter.ConvertValue<string>(100));
			Assert.Equal("100", Converter.ConvertValue<string>(100L));
			Assert.Equal("100.5", Converter.ConvertValue<string>(100.50));
			Assert.Equal("100.50", Converter.ConvertValue<string>(100.50m));

			Assert.Equal(Gender.Male, Converter.ConvertValue("male", typeof(Gender)));
			Assert.Equal(Gender.Male, Converter.ConvertValue("Male", typeof(Gender)));
			Assert.Equal(Gender.Female, Converter.ConvertValue<Gender>("female"));
			Assert.Equal(Gender.Female, Converter.ConvertValue<Gender>("Female"));
			
			Assert.Equal(Gender.Male, Converter.ConvertValue(0, typeof(Gender)));
			Assert.Equal(Gender.Male, Converter.ConvertValue("0", typeof(Gender)));

			Assert.Equal(Gender.Female, Converter.ConvertValue<Gender>(1));
			Assert.Equal(Gender.Female, Converter.ConvertValue<Gender>("1"));

			//根据枚举项的 AliasAttribute 值来解析
			Assert.Equal(Gender.Male, Converter.ConvertValue<Gender>("M"));
			Assert.Equal(Gender.Female, Converter.ConvertValue<Gender>("F"));

			//根据枚举项的 DescriptionAttribute 值来解析
			Assert.Equal(Gender.Male, Converter.ConvertValue<Gender>("先生"));
			Assert.Equal(Gender.Female, Converter.ConvertValue<Gender>("女士"));
		}

		[Fact]
		public void GetValueTest()
		{
			var employee = new Employee()
			{
				Name = "Jason",
				Gender = Gender.Male,
				Salary = 10000.01m,
				HomeAddress = new Address
				{
					CountryId = 123,
					City = "ShenZhen",
					Detail = "****",
				},
			};

			Assert.Equal("Jason", Converter.GetValue(employee, "Name"));
			Assert.Equal("ShenZhen", Converter.GetValue(employee, "HomeAddress.City"));

			Converter.SetValue(employee, "Name", "Lucky");
			Assert.Equal("Lucky", Converter.GetValue(employee, "Name"));

			Converter.SetValue(employee, "HomeAddress.City", "XinHua");
			Assert.Equal("XinHua", Converter.GetValue(employee, "HomeAddress.City"));

			var department = new Department("Develop");
			department.AddEmployee(employee);

			var empX = department[0];
			Assert.NotNull(empX);
			var empY = department["Lucky"];
			Assert.NotNull(empY);

			empX = (Employee)Converter.GetValue(department, "[0]");
			Assert.NotNull(empX);

			empY = (Employee)Converter.GetValue(department, "['Lucky']");
			Assert.NotNull(empX);
		}

		[Fact]
		public void ToHexStringTest()
		{
			var source = new byte[16];

			for(int i = 0; i < source.Length; i++)
				source[i] = (byte)i;

			var hexString1 = Converter.ToHexString(source);
			var hexString2 = Converter.ToHexString(source, '-');

			Assert.Equal("000102030405060708090A0B0C0D0E0F", hexString1);
			Assert.Equal("00-01-02-03-04-05-06-07-08-09-0A-0B-0C-0D-0E-0F", hexString2);

			var bytes1 = Converter.FromHexString(hexString1);
			var bytes2 = Converter.FromHexString(hexString2, '-');

			Assert.Equal(source.Length, bytes1.Length);
			Assert.Equal(source.Length, bytes2.Length);

			Assert.True(BinaryComparer.Default.Equals(source, bytes1));
			Assert.True(BinaryComparer.Default.Equals(source, bytes2));
		}
	}
}