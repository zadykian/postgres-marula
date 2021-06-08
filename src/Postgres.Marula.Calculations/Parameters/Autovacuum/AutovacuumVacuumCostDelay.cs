using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Autovacuum
{
	/// <summary>
	/// [autovacuum_vacuum_cost_delay]
	/// Specifies the cost delay value that will be used in automatic vacuum operations.
	/// </summary>
	internal class AutovacuumVacuumCostDelay : TimeSpanParameterBase
	{
		public AutovacuumVacuumCostDelay(ILogger<TimeSpanParameterBase> logger) : base(logger)
		{
		}

		/// <inheritdoc />
		protected override ValueTask<PositiveTimeSpan> CalculateValueAsync()
			=> TimeSpan
				.FromMilliseconds(2)
				.To(timespan => ValueTask.FromResult((PositiveTimeSpan) timespan));
	}
}