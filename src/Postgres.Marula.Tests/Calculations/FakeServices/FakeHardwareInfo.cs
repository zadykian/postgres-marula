using System.Threading.Tasks;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable BuiltInTypeReferenceStyle
using CoresCount = System.Byte;

namespace Postgres.Marula.Tests.Calculations.FakeServices
{
	/// <inheritdoc />
	internal class FakeHardwareInfo : IHardwareInfo
	{
		/// <inheritdoc />
		Task<Memory> IHardwareInfo.TotalRam() => Task.FromResult(16 * Memory.Gigabyte);

		/// <inheritdoc />
		Task<CoresCount> IHardwareInfo.CpuCoresCount() => Task.FromResult((CoresCount) 8);
	}
}