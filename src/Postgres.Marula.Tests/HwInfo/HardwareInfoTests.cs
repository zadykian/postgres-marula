using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.HwInfo
{
	/// <summary>
	/// Hardware agent tests
	/// </summary>
	internal class HardwareInfoTests : SingleComponentTestFixtureBase<HwInfoAppComponent>
	{
		/// <summary>
		/// Get total RAM size.
		/// </summary>
		[Test]
		public async Task GetTotalMemoryTest()
		{
			var hardwareInfo = GetService<IHardwareInfo>();
			var totalMemory = await hardwareInfo.TotalRam();
			Assert.AreNotEqual(Memory.Zero, totalMemory);
		}

		/// <summary>
		/// Get CPU cores count.
		/// </summary>
		[Test]
		public async Task GetCpuCoresCountTest()
		{
			var hardwareInfo = GetService<IHardwareInfo>();
			var coresCount = await hardwareInfo.CpuCoresCount();
			Assert.AreNotEqual(0, coresCount);
		}
	}
}