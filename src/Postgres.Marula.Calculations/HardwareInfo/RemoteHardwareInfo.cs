using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable BuiltInTypeReferenceStyle
using CoresCount = System.Byte;

namespace Postgres.Marula.Calculations.HardwareInfo
{
	/// <inheritdoc />
	/// <remarks>
	/// This implementations accesses remote agent via HTTP.
	/// </remarks>
	internal class RemoteHardwareInfo : IHardwareInfo
	{
		private readonly HttpClient httpClient;

		public RemoteHardwareInfo(ICalculationsConfiguration configuration)
			=> httpClient = configuration
				.General()
				.AgentApiUri()
				.To(endpoint => new HttpClient {BaseAddress = endpoint});

		/// <inheritdoc />
		async Task<Memory> IHardwareInfo.TotalRam()
			=> await PerformRequestAsync<Memory>(HttpMethod.Get, "HardwareInfo/GetTotalRam");

		/// <inheritdoc />
		async Task<CoresCount> IHardwareInfo.CpuCoresCount()
			=> await PerformRequestAsync<CoresCount>(HttpMethod.Get, "HardwareInfo/GetCpuCoresCount");

		/// <summary>
		/// Send HTTP request to remote agent.
		/// </summary>
		private async Task<TResponse> PerformRequestAsync<TResponse>(HttpMethod httpMethod, NonEmptyString route)
		{
			var httpResponseMessage = await httpClient.SendAsync(new(httpMethod, route));
			var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions { Converters = { MemoryConverter.Instance } };
			return JsonSerializer.Deserialize<TResponse>(responseBody, options)!;
		}

		/// <summary>
		/// JSON read-only converter for <see cref="Memory"/> type.
		/// </summary>
		private sealed class MemoryConverter : JsonConverter<Memory>
		{
			private MemoryConverter()
			{
			}

			/// <summary>
			/// Converter instance.
			/// </summary>
			public static MemoryConverter Instance { get; } = new();

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
}