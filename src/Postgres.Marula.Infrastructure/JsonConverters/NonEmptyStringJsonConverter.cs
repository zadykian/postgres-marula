using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Infrastructure.JsonConverters
{
	/// <summary>
	/// JSON converter for <see cref="NonEmptyString"/> type.
	/// </summary>
	public class NonEmptyStringJsonConverter : JsonConverter<NonEmptyString>
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