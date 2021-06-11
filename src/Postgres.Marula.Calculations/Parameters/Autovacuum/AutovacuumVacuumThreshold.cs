using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;

// ReSharper disable UnusedType.Global
// ReSharper disable BuiltInTypeReferenceStyle
using TuplesCount = System.UInt32;

namespace Postgres.Marula.Calculations.Parameters.Autovacuum
{
	/// <summary>
	/// [autovacuum_vacuum_threshold]
	/// Specifies the minimum number of updated or deleted tuples
	/// needed to trigger a vacuum in any one table.
	/// </summary>
	internal class AutovacuumVacuumThreshold : IntegerParameterBase
	{
		private readonly IDatabaseServer databaseServer;

		public AutovacuumVacuumThreshold(
			IDatabaseServer databaseServer,
			ILogger<AutovacuumVacuumThreshold> logger) : base(logger)
			=> this.databaseServer = databaseServer;

		/// <inheritdoc />
		/// <remarks>
		/// Value calculated based on table size EV and can be represented by formula:
		/// autovacuum_vacuum_threshold = max(50, 0.01 * {table_size_expected_value})
		/// </remarks>
		protected override async ValueTask<TuplesCount> CalculateValueAsync()
		{
			var averageTableSize = await databaseServer.GetAverageTableSizeAsync();
			return Math.Max(50, (TuplesCount) 0.01 * averageTableSize);
		}
	}
}