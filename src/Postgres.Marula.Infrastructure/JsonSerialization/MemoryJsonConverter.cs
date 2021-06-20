using System;
using System.Text.Json;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Infrastructure.JsonSerialization
{
	/// <summary>
	/// JSON read-only converter for <see cref="Memory"/> type.
	/// </summary>
	internal class MemoryConverter : CustomJsonConverter<Memory>
	{
		/// <inheritdoc />
		public override Memory Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			reader.Read(); // skip start object: '{'
			reader.Read(); // skip property name: 'totalBytes'
			var totalBytesValue = reader.GetUInt64();
			while (reader.TokenType != JsonTokenType.EndObject) reader.Read();
			return new(totalBytesValue);
		}

		/// <inheritdoc />
		public override void Write(
			Utf8JsonWriter writer,
			Memory value,
			JsonSerializerOptions options) => throw new NotSupportedException();
	}
}