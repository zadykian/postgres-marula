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
	/// [max_parallel_workers_per_gather]
	/// Sets the maximum number of parallel processes per executor node.
	/// </summary>
	internal class MaxParallelWorkersPerGather : IntegerParameterBase
	{
		private readonly IHardwareInfo hardwareInfo;

		public MaxParallelWorkersPerGather(
			IHardwareInfo hardwareInfo,
			ILogger<IntegerParameterBase> logger) : base(logger)
			=> this.hardwareInfo = hardwareInfo;

		/// <inheritdoc />
		protected override async ValueTask<CoresCount> CalculateValueAsync()
		{
			var cpuCoresCount = await hardwareInfo.GetCpuCoresCountAsync();
			return (CoresCount) Math.Min(4, 0.5 * cpuCoresCount);
		}
	}
}