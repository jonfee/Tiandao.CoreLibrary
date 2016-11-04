using System;
using System.Collections.Generic;

namespace Tiandao.Common
{
    public interface IAccumulator
    {
		long Increment(string key, int interval = 1);

		long Decrement(string key, int interval = 1);
	}
}