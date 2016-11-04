using System;

namespace Tiandao.Services
{
    public interface IMatcher
    {
		bool Match(object target, object parameter);
	}
}