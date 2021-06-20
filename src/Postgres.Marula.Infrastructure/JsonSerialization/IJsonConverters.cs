using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Postgres.Marula.Infrastructure.JsonSerialization
{
	/// <summary>
	/// JSON converters.
	/// </summary>
	public interface IJsonConverters
	{
		/// <summary>
		/// Get all JSON converters defined in loaded assemblies
		/// which are inherited from <see cref="CustomJsonConverter{T}"/>.
		/// </summary>
		IEnumerable<JsonConverter> All();
	}
}