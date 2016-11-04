using System;
using System.Collections.Generic;
using System.Linq;

using Tiandao.ComponentModel;

namespace Tiandao.Test
{
    public enum Gender
    {
		/// <summary>
		/// 先生
		/// </summary>
		[Alias("M")]
		[Description("${Text.Gender.Male}")]
		Male,

		/// <summary>
		/// 女士
		/// </summary>
		[Alias("F")]
		[Description("${Text.Gender.Female}")]
		Female
    }
}