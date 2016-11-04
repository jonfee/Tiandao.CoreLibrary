using System;

namespace Tiandao.Services
{
    public interface IMatchable<T> : IMatchable
    {
		bool IsMatch(T parameter);
	}
}