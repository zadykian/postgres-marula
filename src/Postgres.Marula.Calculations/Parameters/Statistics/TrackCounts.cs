using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Statistics
{
	/// <summary>
	/// [track_counts]
	/// Enables collection of statistics on database activity.
	/// This parameter affects autovacuum, because the autovacuum daemon needs the collected information.
	/// </summary>
	internal class TrackCountsParameter : BooleanParameterBase
	{
		public TrackCountsParameter(ILogger<TrackCountsParameter> logger) : base(logger)
		{
		}

		/// <inheritdoc />
		protected override ValueTask<bool> CalculateValueAsync() => ValueTask.FromResult(true);
	}
}