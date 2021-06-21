using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.HwInfo;

// ReSharper disable UnusedType.Global
// ReSharper disable BuiltInTypeReferenceStyle
using CoresCount = System.UInt32;

namespace Postgres.Marula.Calculations.Parameters.ResourceUsage.AsynchronousBehaviour
{
	/// <summary>
	/// [max_parallel_maintenance_workers]
	/// Sets the maximum number of parallel processes per maintenance operation.
	/// </summary>
	internal class MaxParallelMaintenanceWorkers : IntegerParameterBase
	{
		private readonly IHardwareInfo hardwareInfo;

		public MaxParallelMaintenanceWorkers(
			IHardwareInfo hardwareInfo,
			ILogger<IntegerParameterBase> logger) : base(logger)
			=> this.hardwareInfo = hardwareInfo;

		/// <inheritdoc />
		protected override async ValueTask<uint> CalculateValueAsync()
		{
			var cpuCoresCount = await hardwareInfo.CpuCoresCount();
			return (CoresCount) Math.Min(4, 0.5 * cpuCoresCount);
		}
	}
}