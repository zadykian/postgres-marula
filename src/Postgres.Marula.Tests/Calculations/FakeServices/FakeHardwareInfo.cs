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
		Task<Memory> IHardwareInfo.GetTotalRamAsync() => Task.FromResult(32 * Memory.Gigabyte);

		/// <inheritdoc />
		Task<CoresCount> IHardwareInfo.GetCpuCoresCountAsync() => Task.FromResult((CoresCount) 16);
	}
}