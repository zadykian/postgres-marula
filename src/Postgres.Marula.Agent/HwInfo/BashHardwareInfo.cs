using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Agent.HwInfo
{
	/// <inheritdoc />
	/// <remarks>
	/// This implementation uses bash to access hardware info.
	/// </remarks>
	internal class BashHardwareInfo : IHardwareInfo
	{
		/// <inheritdoc />
		Memory IHardwareInfo.TotalRam() => throw new System.NotImplementedException();

		/// <inheritdoc />
		byte IHardwareInfo.CpuCoresCount() => throw new System.NotImplementedException();
	}
}