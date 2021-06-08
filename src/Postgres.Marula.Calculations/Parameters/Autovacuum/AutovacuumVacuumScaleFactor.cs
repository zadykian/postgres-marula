using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Autovacuum
{
	/// <summary>
	/// [autovacuum_vacuum_scale_factor]
	/// Specifies a fraction of the table size to add to
	/// <see cref="AutovacuumVacuumThreshold"/> when deciding whether to trigger a vacuum.
	/// </summary>
	internal class AutovacuumVacuumScaleFactor : FractionParameterBase
	{
		private readonly IDatabaseServer databaseServer;

		public AutovacuumVacuumScaleFactor(
			IDatabaseServer databaseServer,
			ILogger<AutovacuumVacuumScaleFactor> logger) : base(logger)
			=> this.databaseServer = databaseServer;

		/// <inheritdoc />
		/// <remarks>
		/// Fraction calculated based on table size EV:
		/// autovacuum_vacuum_scale_factor = min(0.2, 10^4 / {table_size_expected_value})
		/// </remarks>
		protected override async ValueTask<Fraction> CalculateValueAsync()
		{
			var averageTableSize = await databaseServer.GetAverageTableSizeAsync();
			return Math.Min(0.2M, 10_000M / averageTableSize);
		}
	}
}