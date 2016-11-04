using System;
using System.Collections.Generic;

namespace Tiandao.Caching
{
    public interface ICacheCreator
    {
		object Create(string cacheName, string key, out TimeSpan duration);
	}
}