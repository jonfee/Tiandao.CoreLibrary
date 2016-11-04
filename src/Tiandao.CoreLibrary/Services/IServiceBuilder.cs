using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public interface IServiceBuilder
    {
		object Build(ServiceEntry entry);
	}
}