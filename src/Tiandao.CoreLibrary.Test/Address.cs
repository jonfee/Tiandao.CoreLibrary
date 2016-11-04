using System;
using System.Collections.Generic;
using System.Linq;
using Tiandao.ComponentModel;

namespace Tiandao.Test
{
	[DisplayName("地址")]
	[Description("表示一个地址信息。")]
    public class Address
    {
		public string City
		{
			get;
			set;
		}

		public string Detail
		{
			get;
			set;
		}

		public string PostalCode
		{
			get;
			set;
		}

		public int CountryId
		{
			get;
			set;
		}
	}
}
