using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Base.Dependencies;
using Postgres.Marula.Calculations.Parameters.ResourceUsage.Memory;
using Postgres.Marula.Calculations.ParametersManagement;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Wal
{
	/// <summary>
	/// [wal_buffers]
	/// Sets the number of disk-page buffers in shared memory for WAL.
	/// </summary>
	internal class WalBuffers : MemoryParameterBase
	{
		private readonly IPgSettings pgSettings;

		public WalBuffers(
			IPgSettings pgSettings,
			ILogger<MemoryParameterBase> logger) : base(logger)
			=> this.pgSettings = pgSettings;

		/// <inheritdoc />
		public override IParameterDependencies Dependencies()
			=> ParameterDependencies
				.Empty
				.DependsOn<SharedBuffers>();

		/// <inheritdoc />
		protected override async ValueTask<Memory> CalculateValueAsync()
		{
			var sharedBuffers = await pgSettings.ReadAsync<SharedBuffers, Memory>();
			var fractionOfBuffers = sharedBuffers / 32;
			return fractionOfBuffers.Limit(64 * Memory.Kilobyte, 32 * Memory.Megabyte);
		}
	}
}