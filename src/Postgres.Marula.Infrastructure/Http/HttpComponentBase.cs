using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Infrastructure.Http
{
	/// <summary>
	/// Base type for components which perform HTTP requests to remote API.
	/// </summary>
	public abstract class HttpComponentBase
	{
		private readonly HttpClient httpClient;

		protected HttpComponentBase(Uri baseAddress) => httpClient = new() {BaseAddress = baseAddress};

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
				var serializationOptions = ConfigureSerializerOptions(new());
				var serialized = JsonSerializer.Serialize(requestBody, serializationOptions);
				httpRequestMessage.Content = new StringContent(serialized);
			}

			try
			{
				httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
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
			var deserializationOptions = ConfigureSerializerOptions(new());
			return JsonSerializer.Deserialize<TResponse>(responseBody, deserializationOptions)!;
		}

		/// <summary>
		/// Wrap <paramref name="occuredError"/> into other exception before
		/// rethrowing by <see cref="PerformRequestAsync{TResponse}"/> method. 
		/// </summary>
		protected virtual Exception WrapException(Exception occuredError)
			=> occuredError;

		/// <summary>
		/// Configure <see cref="JsonSerializerOptions"/> for request body serialization
		/// and response body deserialization.
		/// </summary>
		protected virtual JsonSerializerOptions ConfigureSerializerOptions(
			JsonSerializerOptions serializerOptions)
			=> serializerOptions;

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