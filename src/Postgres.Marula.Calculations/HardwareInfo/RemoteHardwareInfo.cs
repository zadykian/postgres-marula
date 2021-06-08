using System.Threading.Tasks;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.HardwareInfo
{
	/// <inheritdoc />
	/// <remarks>
	/// This implementations accesses remote agent via HTTP.
	/// </remarks>
	internal class RemoteHardwareInfo : IHardwareInfo
	{
		/// <inheritdoc />
		async Task<Memory> IHardwareInfo.TotalRam() => throw new System.NotImplementedException();

		/// <inheritdoc />
		async Task<byte> IHardwareInfo.CpuCoresCount() => throw new System.NotImplementedException();
	}
}