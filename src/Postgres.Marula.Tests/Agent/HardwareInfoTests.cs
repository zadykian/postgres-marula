using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.Agent;
using Postgres.Marula.Agent.HwInfo;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.Agent
{
	/// <summary>
	/// Hardware agent tests
	/// </summary>
	internal class HardwareInfoTests : SingleComponentTestFixtureBase<AgentAppComponent>
	{
		/// <summary>
		/// Get total RAM size.
		/// </summary>
		[Test]
		public async Task GetTotalMemoryTest()
		{
			var hardwareInfo = GetService<IHardwareInfo>();
			var totalMemory = await hardwareInfo.TotalRam();
			Assert.AreNotEqual(0, totalMemory.TotalBytes);
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