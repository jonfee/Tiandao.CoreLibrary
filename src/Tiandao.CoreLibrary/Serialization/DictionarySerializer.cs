using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Tiandao.Common;

namespace Tiandao.Serialization
{
    public class DictionarySerializer : IDictionarySerializer
	{
		#region 单例字段

		public static readonly DictionarySerializer Default = new DictionarySerializer();

		#endregion

		#region 公共方法

		public IDictionary Serialize(object graph)
		{
			var dictionary = new Dictionary<string, object>();
			this.Serialize(graph, dictionary);
			return dictionary;
		}

		public void Serialize(object graph, IDictionary dictionary)
		{
			if(graph == null)
				return;

			if(dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			dictionary.Add("@type", graph.GetType().AssemblyQualifiedName);

			var properties = graph.GetType().GetProperties();

			foreach(var property in properties)
			{
				if(!property.CanRead)
					continue;

				if(TypeExtension.IsScalarType(property.PropertyType))
					dictionary.Add(property.Name.ToLowerInvariant(), property.GetValue(graph));
			}
		}

		public object Deserialize(IDictionary dictionary)
		{
			return this.Deserialize(dictionary, null);
		}

		public object Deserialize(IDictionary dictionary, Type type)
		{
			return this.Deserialize(dictionary, type, null);
		}

		public object Deserialize(IDictionary dictionary, Type type, Action<Converter.ObjectResolvingContext> resolve)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			return this.Deserialize<object>(dictionary, () => Activator.CreateInstance(type), resolve);
		}

		public T Deserialize<T>(IDictionary dictionary)
		{
			return (T)this.Deserialize(dictionary, typeof(T));
		}

		public T Deserialize<T>(IDictionary dictionary, Func<T> creator, Action<Converter.ObjectResolvingContext> resolve)
		{
			if(dictionary == null)
				return default(T);

			var result = creator != null ? creator() : Activator.CreateInstance<T>();

			if(resolve == null)
			{
				resolve = ctx =>
				{
					if(ctx.Direction == Converter.ObjectResolvingDirection.Get)
					{
						ctx.Value = ctx.GetMemberValue();

						if(ctx.Value == null)
						{
							ctx.Value = Activator.CreateInstance(ctx.MemberType);

							if(ctx.Member is FieldInfo)
							{
								((FieldInfo)ctx.Member).SetValue(ctx.Container, ctx.Value);
							}
							else if(ctx.Member is PropertyInfo)
							{
								((PropertyInfo)ctx.Member).SetValue(ctx.Container, ctx.Value);
							}

//							switch(ctx.Member.MemberType)
//							{
//								case MemberTypes.Field:
//									((FieldInfo)ctx.Member).SetValue(ctx.Container, ctx.Value);
//									break;
//								case MemberTypes.Property:
//									((PropertyInfo)ctx.Member).SetValue(ctx.Container, ctx.Value);
//									break;
//							}
						}
					}
				};
			}

			foreach(DictionaryEntry entry in dictionary)
			{
				if(entry.Key == null)
					continue;

				Converter.SetValue(result, entry.Key.ToString(), entry.Value, resolve);
			}

			return result;
		}

		#endregion
	}
}
