using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Tiandao.ComponentModel
{
    public class BooleanConverter : System.ComponentModel.BooleanConverter
	{
		private static readonly Regex _digits = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$", RegexOptions.Compiled);

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string buffer = (value ?? string.Empty).ToString().Trim();

			if(!string.IsNullOrEmpty(buffer))
			{
				if(_digits.IsMatch(buffer))
				{
					var number = Convert.ToInt32(buffer, CultureInfo.InvariantCulture);

					return number != 0;
				}
			}

			return base.ConvertFrom(context, culture, value);
		}
	}
}
