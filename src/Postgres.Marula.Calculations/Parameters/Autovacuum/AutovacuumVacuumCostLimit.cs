using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Autovacuum.Bloat;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParametersManagement;

// ReSharper disable UnusedType.Global
// ReSharper disable BuiltInTypeReferenceStyle
using CostLimit = System.UInt32;
using WorkersCount = System.UInt32;

namespace Postgres.Marula.Calculations.Parameters.Autovacuum
{
	/// <summary>
	/// [autovacuum_vacuum_cost_limit]
	/// Specifies the cost limit value that will be used in automatic vacuum operations.
	/// </summary>
	internal class AutovacuumVacuumCostLimit : IntegerParameterBase
	{
		private readonly IBloatAnalysis bloatAnalysis;
		private readonly IPgSettings pgSettings;

		public AutovacuumVacuumCostLimit(
			IBloatAnalysis bloatAnalysis,
			IPgSettings pgSettings,
			ILogger<IntegerParameterBase> logger) : base(logger)
		{
			this.bloatAnalysis = bloatAnalysis;
			this.pgSettings = pgSettings;
		}

		/// <inheritdoc />
		protected override async ValueTask<CostLimit> CalculateValueAsync()
		{
			// todo: perform calculations based on BloatCoefficients.
			await bloatAnalysis.ExecuteAsync();

			var autovacuumMaxWorkers = await pgSettings.ReadAsync<AutovacuumMaxWorkers, WorkersCount>();
			return 500 * autovacuumMaxWorkers;
		}
	}
}