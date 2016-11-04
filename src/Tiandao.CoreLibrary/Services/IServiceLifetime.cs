using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public interface IServiceLifetime
    {
		bool IsAlive(ServiceEntry entry);
	}
}