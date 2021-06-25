using System.Text.Json.Serialization;

namespace Postgres.Marula.Infrastructure.JsonSerialization
{
	/// <inheritdoc />
	public abstract class CustomJsonConverter<T> : JsonConverter<T>
	{
	}
}