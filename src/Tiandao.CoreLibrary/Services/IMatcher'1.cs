using System;
using System.Collections.Generic;

namespace Tiandao.Services
{
    public interface IMatcher<T> : IMatcher
    {
		bool Match(object target, T parameter);
	}
}