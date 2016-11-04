using System;
using System.IO;

namespace Tiandao.Serialization
{
    public interface ITextSerializer : ISerializer
    {
		string Serialize(object graph, TextSerializationSettings settings = null);
		void Serialize(TextWriter writer, object graph, TextSerializationSettings settings = null);

		object Deserialize(string text);
		object Deserialize(string text, Type type);
		T Deserialize<T>(string text);

		object Deserialize(TextReader reader);
		object Deserialize(TextReader reader, Type type);
		T Deserialize<T>(TextReader reader);
	}
}