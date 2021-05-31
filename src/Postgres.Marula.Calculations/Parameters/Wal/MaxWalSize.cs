using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Exceptions;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Wal
{
	// internal interface IDbServerParameters
	// {
	// 	Task<TValue> ReadAsync<TValue>(NonEmptyString parameterName);
	// }

	/// <summary>
	/// [max_wal_size]
	/// Maximum size to let the WAL grow during automatic checkpoints.
	/// This is a soft limit - WAL size can exceed max_wal_size under special circumstances.
	/// </summary>
	internal class MaxWalSize : MemoryParameterBase
	{
		private readonly ICalculationsConfiguration configuration;
		private readonly ISystemStorage systemStorage;
		private readonly IDatabaseServer databaseServer;

		public MaxWalSize(
			ICalculationsConfiguration configuration,
			ISystemStorage systemStorage,
			IDatabaseServer databaseServer,
			ILogger<MaxWalSize> logger) : base(logger)
		{
			this.systemStorage = systemStorage;
			this.databaseServer = databaseServer;
			this.configuration = configuration;
		}

		/// <inheritdoc />
		/// <remarks>
		/// Server needs to keep WAL files starting at the moment of the last completed checkpoint
		/// plus the files accumulated during the current checkpoint.
		/// But for before Postgres 11 server also retained files from the last but one checkpoint.
		/// </remarks>
		protected override async ValueTask<Memory> CalculateValueAsync()
		{
			var walTrafficPerSecond = await GetWalTrafficPerSecond();

			var currentServerVersion = await databaseServer.GetPostgresVersionAsync();
			var multiplier = currentServerVersion >= new Version(11, 0) ? 1 : 2;
			
			// todo: get checkpoint_completion_target value
		}

		/// <summary>
		/// Get average WAL traffic per second.
		/// </summary>
		/// <exception cref="ParameterValueCalculationException">
		/// Rises when there are no LSN history in configured time window.
		/// </exception>
		private async Task<Memory> GetWalTrafficPerSecond()
		{
			var lsnHistoryEntries = await configuration
				.MovingAverageWindow()
				.To(systemStorage.GetLsnHistoryAsync)
				.ToArrayAsync();

			// At least two values are required to calculate average.
			if (lsnHistoryEntries.Length < 2) throw Error.NoLsnHistory();

			return lsnHistoryEntries
				.Pairwise()
				.Select(pair => pair.Left.TrafficPerSecondBefore(pair.Right))
				.Average(memory => (double) memory.TotalBytes)
				.To(averageBytes => new Memory((ulong)averageBytes));
		}
	}
}