using System;
using System.Collections.Generic;

namespace Tiandao.Text
{
    public abstract class TemplateEvaluatorBase : ITemplateEvaluator
	{
		#region 私有字段

		private string _scheme;

		#endregion

		#region 公共属性

		public string Scheme
		{
			get
			{
				return _scheme;
			}
		}

		#endregion

		#region 构造方法

		protected TemplateEvaluatorBase(string scheme)
		{
			if(string.IsNullOrWhiteSpace(scheme))
				throw new ArgumentNullException(nameof(scheme));

			_scheme = scheme.Trim();
		}

		#endregion

		#region 抽象方法

		public abstract object Evaluate(TemplateEvaluatorContext context);

		#endregion
	}
}