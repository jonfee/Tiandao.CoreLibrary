using System;
using System.Collections.Generic;

namespace Tiandao.Serialization
{
    public class JsonSerializationWriter : TextSerializationWriterBase
    {
		#region 构造方法

		public JsonSerializationWriter()
		{

		}

		#endregion

		#region 写入方法

		protected override void OnWrite(SerializationWriterContext context)
		{
			var writer = this.GetWriter(context);

			if(writer == null)
				throw new System.Runtime.Serialization.SerializationException("Can not obtain a text writer.");

			if(context.Index > 0)
				writer.WriteLine(",");
			else
				writer.WriteLine();

			var indentText = this.GetIndentText(context.Depth);
			writer.Write(indentText);

			if(context.Member != null)
				writer.Write("\"" + context.MemberName + "\" : ");

			if(context.Value == null || context.IsCircularReference)
			{
				writer.Write("\"\"");
				return;
			}

			var directedValue = string.Empty;
			var isDirectedValue = this.GetDirectedValue(context.Value, out directedValue);

			if(isDirectedValue)
			{
				writer.Write("\"" + directedValue + "\"");
			}
			else
			{
				if(context.IsCollection)
					writer.Write(writer.NewLine + indentText + "[");
				else
					writer.Write("{");
			}

			context.Terminated = isDirectedValue;
		}

		#endregion

		#region 重写方法

		protected override void OnWrote(SerializationWriterContext context)
		{
			if(context.Terminated || context.IsCircularReference)
				return;

			var writer = this.GetWriter(context);

			if(writer == null)
				return;

			if(context.IsCollection)
				writer.Write(writer.NewLine + this.GetIndentText(context.Depth) + "]");
			else
				writer.Write(writer.NewLine + this.GetIndentText(context.Depth) + "}");
		}

		#endregion
	}
}