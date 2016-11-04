using System;

namespace Tiandao.Options
{
    public interface IOptionLoader
    {
		void Load(IOptionProvider provider);

		void Unload(IOptionProvider provider);
	}
}