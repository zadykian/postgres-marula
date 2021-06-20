using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Infrastructure.JsonSerialization
{
	/// <inheritdoc />
	internal class JsonConverters  : IJsonConverters
	{
		/// <inheritdoc />
		IEnumerable<JsonConverter> IJsonConverters.All()
			=> AppDomain
				.CurrentDomain
				.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(type =>
					!type.IsAbstract
					&& type.IsGenericType
					&& type.BaseType?.GetGenericTypeDefinition() == typeof(CustomJsonConverter<>))
				.Select(Activator.CreateInstance)
				.Cast<JsonConverter>()
				.Add(new JsonStringEnumConverter());
	}
}