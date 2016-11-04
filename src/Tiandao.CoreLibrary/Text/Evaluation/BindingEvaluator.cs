using System;
using System.Collections.Generic;

namespace Tiandao.Text.Evaluation
{
    public class BindingEvaluator : TemplateEvaluatorBase
	{
		#region 构造函数

		public BindingEvaluator() : base("binding")
		{

		}

		public BindingEvaluator(string scheme) : base(scheme)
		{

		}

		#endregion

		#region 评估方法

		public override object Evaluate(TemplateEvaluatorContext context)
		{
			if(context.Data == null)
				return null;

			if(string.IsNullOrWhiteSpace(context.Text))
				return context.Data;

			var index = context.Text.IndexOf('#');
			var result = Common.Converter.GetValue(context.Data, (index > 0 ? context.Text.Substring(0, index) : context.Text));

			if(index > 0 && index < context.Text.Length - 1)
				return string.Format("{0:" + context.Text.Substring(index + 1) + "}", result);

			return result;
		}

		#endregion
	}
}
