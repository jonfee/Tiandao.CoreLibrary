using System;
using System.Collections.Generic;

namespace Tiandao.Text.Evaluation
{
    public class RandomEvaluator : TemplateEvaluatorBase
	{
		#region 构造方法

		public RandomEvaluator() : base("random")
		{

		}

		public RandomEvaluator(string scheme) : base(scheme)
		{

		}

		#endregion

		#region 重写方法

		public override object Evaluate(TemplateEvaluatorContext context)
		{
			if(string.IsNullOrWhiteSpace(context.Text))
				return this.GetDefaultRandom();

			switch(context.Text.ToLowerInvariant())
			{
				case "byte":
					return Common.RandomGenerator.Generate(1)[0].ToString();
				case "short":
				case "int16":
					return ((ushort)Common.RandomGenerator.GenerateInt32()).ToString();
				case "int":
				case "int32":
					return ((uint)Common.RandomGenerator.GenerateInt32()).ToString();
				case "long":
				case "int64":
					return ((ulong)Common.RandomGenerator.GenerateInt64()).ToString();
				case "guid":
					return Guid.NewGuid().ToString("n");
			}

			int length;

			if(Common.Converter.TryConvertValue<int>(context.Text, out length))
				return Common.RandomGenerator.GenerateString(Math.Max(length, 1));

			return this.GetDefaultRandom();
		}

		#endregion

		#region 私有方法

		private string GetDefaultRandom()
		{
			return Common.RandomGenerator.GenerateString(6);
		}

		#endregion
	}
}