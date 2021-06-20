using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.JsonSerialization;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Infrastructure.Http
{
	/// <summary>
	/// Base type for components which perform HTTP requests to remote API.
	/// </summary>
	public abstract class HttpComponentBase
	{
		private readonly HttpClient httpClient;
		private readonly IJsonConverters jsonConverters;

		protected HttpComponentBase(
			Uri baseAddress,
			IJsonConverters jsonConverters)
		{
			httpClient = new() {BaseAddress = baseAddress};
			this.jsonConverters = jsonConverters;
		}

		/// <summary>
		/// Send HTTP request to remote API.
		/// </summary>
		protected async Task<TResponse> PerformRequestAsync<TResponse>(
			HttpMethod httpMethod,
			NonEmptyString route,
			object? requestBody = null)
		{
			HttpResponseMessage httpResponseMessage;

			var httpRequestMessage = new HttpRequestMessage(httpMethod, route);

			if (requestBody is not (null or Unit))
			{
				var serialized = JsonSerializer.Serialize(requestBody, Options());
				httpRequestMessage.Content = new StringContent(serialized);
			}

			try
			{
				httpResponseMessage = await httpClient
					.SendAsync(httpRequestMessage)
					.ConfigureAwait(false); // todo
			}
			catch (Exception exception)
			{
				var wrapped = WrapException(exception);
				if (ReferenceEquals(exception, wrapped)) throw;
				throw wrapped;
			}

			if (typeof(TResponse) == typeof(Unit))
			{
				return Unit.Instance;
			}

			var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<TResponse>(responseBody, Options())!;
		}

		/// <summary>
		/// Get <see cref="JsonSerializerOptions"/> to serialize request body and deserialize response body. 
		/// </summary>
		private JsonSerializerOptions Options()
		{
			var serializerOptions = new JsonSerializerOptions();
			jsonConverters.All().ForEach(serializerOptions.Converters.Add);
			return serializerOptions;
		}

		/// <summary>
		/// Wrap <paramref name="occuredError"/> into other exception before
		/// rethrowing by <see cref="PerformRequestAsync{TResponse}"/> method. 
		/// </summary>
		protected virtual Exception WrapException(Exception occuredError)
			=> occuredError;

		/// <summary>
		/// Representation of empty type.
		/// </summary>
		protected sealed class Unit
		{
			private Unit()
			{
			}

			/// <summary>
			/// Instance of <see cref="Unit"/> type.
			/// </summary>
			public static dynamic Instance { get; } = new Unit();
		}
	}
}