using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Statistics
{
	/// <summary>
	/// Enables collection of statistics on database activity.
	/// This parameter affects autovacuum, because the autovacuum daemon needs the collected information.
	/// </summary>
	internal class TrackCountsParameter : ParameterBase<BooleanParameterValue, bool>
	{
		/// <inheritdoc />
		public override NonEmptyString Name => "track_counts";

		/// <inheritdoc />
		protected override bool CalculateValue() => true;
	}
}