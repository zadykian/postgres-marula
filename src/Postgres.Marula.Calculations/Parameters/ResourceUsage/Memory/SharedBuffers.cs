using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.HwInfo;

// ReSharper disable UnusedType.Global
using Mem = Postgres.Marula.Infrastructure.TypeDecorators.Memory;

namespace Postgres.Marula.Calculations.Parameters.ResourceUsage.Memory
{
	/// <summary>
	/// [shared_buffers]
	/// Sets the amount of memory the database server uses for shared memory buffers.
	/// </summary>
	internal class SharedBuffers : MemoryParameterBase
	{
		private readonly IHardwareInfo hardwareInfo;

		public SharedBuffers(
			IHardwareInfo hardwareInfo,
			ILogger<SharedBuffers> logger) : base(logger)
			=> this.hardwareInfo = hardwareInfo;

		/// <inheritdoc />
		protected override async ValueTask<Mem> CalculateValueAsync()
		{
			var totalRamSize = await hardwareInfo.GetTotalRamAsync();
			return 0.25 * totalRamSize;
		}
	}
}