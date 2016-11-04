using System;
using System.Collections.Generic;

namespace Tiandao.Common
{
    public static class GuidExtension
    {
		public static bool IsEmpty(this Guid guid)
		{
			return guid.Equals(Guid.Empty);
		}

		public static bool IsNullOrEmpty(this Guid? guid)
		{
			return guid == null || guid.Equals(Guid.Empty);
		}
	}
}