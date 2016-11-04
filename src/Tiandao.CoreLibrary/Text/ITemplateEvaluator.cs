using System;
using System.Collections.Generic;

namespace Tiandao.Text
{
    public interface ITemplateEvaluator
    {
		string Scheme
		{
			get;
		}

		object Evaluate(TemplateEvaluatorContext context);
	}
}