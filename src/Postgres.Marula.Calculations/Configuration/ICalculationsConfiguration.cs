using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Configuration
{
	/// <summary>
	/// Application configuration.
	/// </summary>
	internal interface ICalculationsConfiguration
	{
		/// <summary>
		/// Get parameters recalculation interval. 
		/// </summary>
		PositiveTimeSpan RecalculationInterval();

		/// <summary>
		/// Automatic database's parameters adjustment is enabled. 
		/// </summary>
		bool AutoAdjustmentIsEnabled();

		/// <summary>
		/// <para>
		/// Average bloat factor of all relation in database server
		/// which serves as target for internal algorithms.
		/// </para>
		/// <para>
		/// For example, if average bloat fraction in database
		/// is far more then current parameter value, autovacuum will be tuned
		/// in more aggressive way. 
		/// </para>
		/// </summary>
		Fraction TargetRelationsBloatFraction();
	}
}