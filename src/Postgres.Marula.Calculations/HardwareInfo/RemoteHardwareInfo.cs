using System.Net.Http;
using System.Text.Json;
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
			return JsonSerializer.Deserialize<TResponse>(responseBody)!;
		}
	}
}