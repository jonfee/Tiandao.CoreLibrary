using System;

namespace Tiandao.Serialization
{
	[Flags]
    public enum SerializationBehavior
    {
		None = 0,

		IgnoreDefaultValue = 1,
	}
}