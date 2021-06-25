using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.HwInfo
{
	/// <summary>
	/// Base class for <see cref="IHardwareInfo"/> implementations.
	/// </summary>
	internal abstract class HardwareInfoTestBase : SingleComponentTestFixtureBase<HwInfoAppComponent>
	{
		/// <summary>
		/// Get total RAM size.
		/// </summary>
		protected async Task GetTotalMemoryTestImpl()
		{
			var hardwareInfo = GetService<IHardwareInfo>();
			var totalMemory = await hardwareInfo.GetTotalRamAsync();
			Assert.AreNotEqual(Memory.Zero, totalMemory);
		}

		/// <summary>
		/// Get CPU cores count.
		/// </summary>
		protected async Task GetCpuCoresCountTestImpl()
		{
			var hardwareInfo = GetService<IHardwareInfo>();
			var coresCount = await hardwareInfo.GetCpuCoresCountAsync();
			Assert.AreNotEqual(0, coresCount);
		}
	}
}