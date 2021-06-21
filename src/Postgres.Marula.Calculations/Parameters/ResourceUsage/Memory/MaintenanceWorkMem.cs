using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.HwInfo;

// ReSharper disable UnusedType.Global
using Mem = Postgres.Marula.Infrastructure.TypeDecorators.Memory;

namespace Postgres.Marula.Calculations.Parameters.ResourceUsage.Memory
{
	/// <summary>
	/// [maintenance_work_mem]
	/// Specifies the maximum amount of memory to be used by maintenance operations,
	/// such as VACUUM, CREATE INDEX, and ALTER TABLE ADD FOREIGN KEY.
	/// </summary>
	internal class MaintenanceWorkMem : MemoryParameterBase
	{
		private readonly IHardwareInfo hardwareInfo;

		public MaintenanceWorkMem(
			IHardwareInfo hardwareInfo,
			ILogger<MaintenanceWorkMem> logger) : base(logger)
			=> this.hardwareInfo = hardwareInfo;

		/// <inheritdoc />
		protected override async ValueTask<Mem> CalculateValueAsync()
		{
			var totalRamSize = await hardwareInfo.TotalRam();
			return 0.05 * totalRamSize;
		}
	}
}