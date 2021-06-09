using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.MemoryUsage
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
		protected override async ValueTask<Memory> CalculateValueAsync()
		{
			var totalRamSize = await hardwareInfo.TotalRam();
			return 0.25 * totalRamSize;
		}
	}
}