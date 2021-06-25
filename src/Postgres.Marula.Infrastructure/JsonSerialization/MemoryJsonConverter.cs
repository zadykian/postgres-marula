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
			JsonSerializerOptions options) => new(reader.GetUInt64());

		/// <inheritdoc />
		public override void Write(
			Utf8JsonWriter writer,
			Memory value,
			JsonSerializerOptions options) => writer.WriteNumberValue(value.TotalBytes);
	}
}