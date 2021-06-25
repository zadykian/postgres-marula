using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.HwInfo;

// ReSharper disable UnusedType.Global
// ReSharper disable BuiltInTypeReferenceStyle
using CoresCount = System.UInt32;

namespace Postgres.Marula.Calculations.Parameters.Autovacuum
{
	/// <summary>
	/// [autovacuum_max_workers]
	/// Specifies the maximum number of autovacuum
	/// processes (other than the autovacuum launcher) that may be running at any one time.
	/// </summary>
	internal class AutovacuumMaxWorkers : IntegerParameterBase
	{
		private readonly IHardwareInfo hardwareInfo;

		public AutovacuumMaxWorkers(
			IHardwareInfo hardwareInfo,
			ILogger<IntegerParameterBase> logger) : base(logger)
			=> this.hardwareInfo = hardwareInfo;

		protected override async ValueTask<CoresCount> CalculateValueAsync()
		{
			var cpuCoresCount = await hardwareInfo.GetCpuCoresCountAsync();
			return (CoresCount) (0.5 * cpuCoresCount);
		}
	}
}