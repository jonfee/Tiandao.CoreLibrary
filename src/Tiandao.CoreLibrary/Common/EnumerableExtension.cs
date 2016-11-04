using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tiandao.Common
{
    public static class EnumerableExtension
    {
	    public static bool HasValue<T>(this IEnumerable<T> source)
	    {
		    return source != null && source.Any();
	    }
    }
}
