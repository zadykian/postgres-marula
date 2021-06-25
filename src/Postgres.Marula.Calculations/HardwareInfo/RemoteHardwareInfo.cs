using System;
using System.Net.Http;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.Exceptions;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.Http;
using Postgres.Marula.Infrastructure.JsonSerialization;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable BuiltInTypeReferenceStyle
using CoresCount = System.Byte;

namespace Postgres.Marula.Calculations.HardwareInfo
{
	/// <inheritdoc cref="IHardwareInfo" />
	/// <remarks>
	/// This implementations accesses remote agent via HTTP.
	/// </remarks>
	internal class RemoteHardwareInfo : HttpComponentBase, IHardwareInfo
	{
		public RemoteHardwareInfo(
			ICalculationsConfiguration configuration,
			IJsonConverters jsonConverters) : base(configuration.General().AgentApiUri(), jsonConverters)
		{
		}

		/// <inheritdoc />
		async Task<Memory> IHardwareInfo.GetTotalRamAsync()
			=> await PerformRequestAsync<Memory>(HttpMethod.Get, "HardwareInfo/GetTotalRam");

		/// <inheritdoc />
		async Task<CoresCount> IHardwareInfo.GetCpuCoresCountAsync()
			=> await PerformRequestAsync<CoresCount>(HttpMethod.Get, "HardwareInfo/GetCpuCoresCount");

		/// <inheritdoc />
		protected override Exception WrapException(Exception occuredError) => Error.FailedToAccessAgent(occuredError);
	}
}