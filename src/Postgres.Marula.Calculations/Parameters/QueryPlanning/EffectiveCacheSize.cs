using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.QueryPlanning
{
	/// <summary>
	/// [effective_cache_size]
	/// Sets the planner's assumption about the effective size of the disk cache
	/// that is available to a single query.
	/// </summary>
	internal class EffectiveCacheSize : MemoryParameterBase
	{
		private readonly IHardwareInfo hardwareInfo;

		public EffectiveCacheSize(
			IHardwareInfo hardwareInfo,
			ILogger<MemoryParameterBase> logger) : base(logger)
			=> this.hardwareInfo = hardwareInfo;

		/// <inheritdoc />
		protected override async ValueTask<Memory> CalculateValueAsync()
		{
			var totalRamSize = await hardwareInfo.TotalRam();
			return 0.75 * totalRamSize;
		}
	}
}