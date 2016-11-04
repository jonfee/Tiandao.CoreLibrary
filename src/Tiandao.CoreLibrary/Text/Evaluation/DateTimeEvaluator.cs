using System;
using System.Collections.Generic;

namespace Tiandao.Text.Evaluation
{
    public class DateTimeEvaluator : TemplateEvaluatorBase
	{
		#region 构造方法

		public DateTimeEvaluator() : base("datetime")
		{

		}

		public DateTimeEvaluator(string scheme) : base(scheme)
		{

		}

		#endregion

		#region 重写方法

		public override object Evaluate(TemplateEvaluatorContext context)
		{
			if(string.IsNullOrWhiteSpace(context.Text))
				return DateTime.Now.ToString();

			return DateTime.Now.ToString(context.Text);
		}

		#endregion
	}
}
