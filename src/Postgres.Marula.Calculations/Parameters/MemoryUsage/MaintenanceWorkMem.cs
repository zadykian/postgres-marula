using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.MemoryUsage
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
		protected override async ValueTask<Memory> CalculateValueAsync()
		{
			var totalRamSize = await hardwareInfo.TotalRam();
			return 0.05 * totalRamSize;
		}
	}
}