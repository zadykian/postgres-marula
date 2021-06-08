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
	/// [autovacuum_naptime]
	/// Specifies the minimum delay between autovacuum runs on any given database.
	/// </summary>
	internal class AutovacuumNaptime : TimeSpanParameterBase
	{
		public AutovacuumNaptime(ILogger<TimeSpanParameterBase> logger) : base(logger)
		{
		}

		/// <inheritdoc />
		protected override ValueTask<PositiveTimeSpan> CalculateValueAsync()
			=> TimeSpan
				.FromSeconds(30)
				.To(timespan => ValueTask.FromResult((PositiveTimeSpan) timespan));
	}
}