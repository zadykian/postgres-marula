using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.HwInfo;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.ResourceUsage.AsynchronousBehaviour
{
	/// <summary>
	/// [max_worker_processes]
	/// Maximum number of concurrent worker processes.
	/// </summary>
	internal class MaxWorkerProcesses : IntegerParameterBase
	{
		private readonly IHardwareInfo hardwareInfo;

		public MaxWorkerProcesses(
			IHardwareInfo hardwareInfo,
			ILogger<IntegerParameterBase> logger) : base(logger)
			=> this.hardwareInfo = hardwareInfo;

		/// <inheritdoc />
		protected override async ValueTask<uint> CalculateValueAsync() => await hardwareInfo.CpuCoresCount();
	}
}