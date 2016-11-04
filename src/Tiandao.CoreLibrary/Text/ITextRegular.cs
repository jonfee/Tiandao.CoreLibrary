using System;
using System.Collections.Generic;

namespace Tiandao.Text
{
    public interface ITextRegular : Services.IMatchable<string>
	{
		bool IsMatch(string text, out string result);
	}
}