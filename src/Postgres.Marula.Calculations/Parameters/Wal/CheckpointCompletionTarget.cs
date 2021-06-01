using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParametersManagement;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Wal
{
	/// <summary>
	/// [checkpoint_completion_target]
	/// Specifies the target of checkpoint completion,
	/// as a fraction of total time between checkpoints.
	/// </summary>
	internal class CheckpointCompletionTarget : FractionParameterBase
	{
		private readonly IPgSettings pgSettings;

		public CheckpointCompletionTarget(
			IPgSettings pgSettings,
			ILogger<FractionParameterBase> logger) : base(logger)
			=> this.pgSettings = pgSettings;

		/// <inheritdoc />
		/// <remarks>
		/// <para>
		/// Value calculated as:
		/// </para>
		/// <para>
		/// checkpoint_completion_target = min(0.9, (checkpoint_timeout - 2 min) / checkpoint_timeout)
		/// </para>
		/// </remarks>
		protected override async ValueTask<Fraction> CalculateValueAsync()
		{
			var checkpointTimeout = await pgSettings.ReadAsync<PositiveTimeSpan>("checkpoint_timeout");
			var basedOnTimeout = (checkpointTimeout - TimeSpan.FromMinutes(2)) / checkpointTimeout;
			return (decimal) Math.Min(0.9, basedOnTimeout);
		}
	}
}