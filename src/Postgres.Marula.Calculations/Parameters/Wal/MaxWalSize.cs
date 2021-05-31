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
		protected override async ValueTask<Memory> CalculateValueAsync()
		{
			var lsnHistoryEntries = await configuration
				.MovingAverageWindow()
				.To(systemStorage.GetLsnHistory)
				.ToArrayAsync();

			if (!lsnHistoryEntries.Any()) throw Error.NoLsnHistory();

			var walTrafficPerSecond = lsnHistoryEntries
				.Pairwise()
				.Select(pair => (
					Memory: pair.Right.WalInsertLocation - pair.Left.WalInsertLocation,
					TimeSpan: new PositiveTimeSpan(pair.Right.LogTimestamp - pair.Left.LogTimestamp)))
				.Select(tuple => new Memory(tuple.Memory.TotalBytes / (ulong) tuple.TimeSpan.TotalMilliseconds))
				.Average(memory => (double) memory.TotalBytes)
				.To(averageBytes => new Memory((ulong)averageBytes));

			var currentServerVersion = await databaseServer.GetPostgresVersionAsync();
			var multiplier = currentServerVersion >= new Version(11, 0) ? 1 : 2;
			
			// todo: get checkpoint_completion_target value
		}
	}
}