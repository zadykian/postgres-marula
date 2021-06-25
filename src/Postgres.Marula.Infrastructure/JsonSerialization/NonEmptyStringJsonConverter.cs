using System;
using System.Text.Json;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Infrastructure.JsonSerialization
{
	/// <summary>
	/// JSON converter for <see cref="NonEmptyString"/> type.
	/// </summary>
	internal class NonEmptyStringJsonConverter : CustomJsonConverter<NonEmptyString>
	{
		/// <inheritdoc />
		public override NonEmptyString Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options) => reader.GetString().ThrowIfNull("value cannot be null");

		/// <inheritdoc />
		public override void Write(
			Utf8JsonWriter writer,
			NonEmptyString value,
			JsonSerializerOptions options) => writer.WriteStringValue(value);
	}
}