using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.HwInfo;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.ResourceUsage.AsynchronousBehaviour
{
	/// <summary>
	/// [max_parallel_workers]
	/// Sets the maximum number of parallel workers that can be active at one time.
	/// </summary>
	internal class MaxParallelWorkers : IntegerParameterBase
	{
		private readonly IHardwareInfo hardwareInfo;

		public MaxParallelWorkers(
			IHardwareInfo hardwareInfo,
			ILogger<IntegerParameterBase> logger) : base(logger)
			=> this.hardwareInfo = hardwareInfo;

		/// <inheritdoc />
		protected override async ValueTask<uint> CalculateValueAsync() => await hardwareInfo.CpuCoresCount();
	}
}