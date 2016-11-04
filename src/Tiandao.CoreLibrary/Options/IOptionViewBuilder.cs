using System;
using System.Collections.Generic;

namespace Tiandao.Options
{
    public interface IOptionViewBuilder
    {
		bool IsValid(IOptionView view);

		IOptionView GetView(IOption option);
	}
}