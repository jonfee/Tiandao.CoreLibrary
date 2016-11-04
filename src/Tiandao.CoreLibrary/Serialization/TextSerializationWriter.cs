using System;
using System.Reflection;

using Tiandao.Common;

namespace Tiandao.Serialization
{
    public class TextSerializationWriter : TextSerializationWriterBase
    {
		#region 构造方法

		public TextSerializationWriter()
		{
		}

		#endregion

		#region 写入方法

		protected override void OnWrite(SerializationWriterContext context)
		{
			var writer = this.GetWriter(context);

			if(writer == null)
				throw new System.Runtime.Serialization.SerializationException("Can not obtain a text writer.");

			if(context.Index >= 0)
				writer.WriteLine();

			var indentText = this.GetIndentText(context.Depth);
			writer.Write(indentText);

			if(context.Member != null)
				writer.Write(context.MemberName + " : ");

			if(context.Value == null)
			{
				writer.Write("<NULL>");
				return;
			}

			if(context.IsCircularReference)
			{
				writer.Write("<Circular Reference>");
				return;
			}

			var directedValue = string.Empty;
			var isDirectedValue = this.GetDirectedValue(context.Value, out directedValue);

			if(isDirectedValue)
			{
				writer.Write(directedValue);
				writer.Write(" (" + this.GetFriendlyTypeName(context.Value.GetType()) + ")");
			}
			else
			{
				writer.Write(this.GetFriendlyTypeName(context.Value.GetType()));

				if(context.IsCollection)
					writer.Write(writer.NewLine + indentText + "[");
				else
					writer.Write(writer.NewLine + indentText + "{");
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

		#region 保护方法

		protected string GetFriendlyTypeName(Type type)
		{
			if(type == null)
				return string.Empty;

			if(type.IsPrimitive() || type == typeof(object) || type == typeof(string))
				return type.Name;

			if(type.IsGenericType())
			{
				string typeName = type.GetGenericTypeDefinition().FullName.Substring(0, type.GetGenericTypeDefinition().FullName.IndexOf('`')) + "<";
				Type[] argumentTypes = type.GetGenericArguments();

				for(int i = 0; i < argumentTypes.Length; i++)
				{
					typeName += this.GetFriendlyTypeName(argumentTypes[i]);

					if(i < argumentTypes.Length - 1)
						typeName += ", ";
				}

				return typeName + ">";
			}

			if(type.FullName.StartsWith("System.", StringComparison.Ordinal) || type.FullName.StartsWith("Tiandao.", StringComparison.Ordinal))
				return type.FullName;
			else
				return type.FullName + ", " + type.GetAssembly().GetName().Name;
		}

		#endregion
	}
}