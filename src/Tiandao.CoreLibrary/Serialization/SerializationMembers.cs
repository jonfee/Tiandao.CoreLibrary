using System;

namespace Tiandao.Serialization
{
	[Flags]
    public enum SerializationMembers
    {
		All = 3,

		Properties = 1,

		Fields = 2,
	}
}