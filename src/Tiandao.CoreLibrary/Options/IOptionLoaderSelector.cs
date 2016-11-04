using System;
using System.Collections.Generic;

namespace Tiandao.Options
{
    public interface IOptionLoaderSelector
    {
		IOptionLoader GetLoader(IOptionProvider provider);
	}
}