using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.HwInfo;

namespace Postgres.Marula.Tests.HwInfo
{
	/// <summary>
	/// Tests of <see cref="IHardwareInfo"/> running locally.
	/// </summary>
	internal class LocalHardwareInfoTests : HardwareInfoTestBase
	{
		/// <inheritdoc cref="HardwareInfoTestBase.GetTotalMemoryTestImpl"/>
		[Test]
		public async Task GetLocalTotalMemoryTest() => await GetTotalMemoryTestImpl();

		/// <inheritdoc cref="HardwareInfoTestBase.GetCpuCoresCountTestImpl"/>
		[Test]
		public async Task GetLocalCpuCoresCountTest() => await GetCpuCoresCountTestImpl();
	}
}