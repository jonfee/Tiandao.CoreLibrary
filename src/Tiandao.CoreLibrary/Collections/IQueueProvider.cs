using System;
using System.Collections.Generic;

namespace Tiandao.Collections
{
    public interface IQueueProvider
    {
		IQueue GetQueue(string name);
	}
}