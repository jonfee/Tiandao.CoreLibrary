using System;
using System.Collections.Generic;

namespace Tiandao.Options
{
    public interface IOptionView
    {
		IOption Option
		{
			get;
		}

		bool IsUsable
		{
			get;
		}
	}
}
