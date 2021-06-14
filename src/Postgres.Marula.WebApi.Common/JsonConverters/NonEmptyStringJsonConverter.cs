using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.WebApi.Common.JsonConverters
{
	/// <summary>
	/// JSON converter for <see cref="NonEmptyString"/> type.
	/// </summary>
	internal class NonEmptyStringJsonConverter : JsonConverter<NonEmptyString>
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