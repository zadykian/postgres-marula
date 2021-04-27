using System.Threading.Tasks;
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
		/// <inheritdoc />
		protected override ValueTask<bool> CalculateValueAsync() => ValueTask.FromResult(true);
	}
}