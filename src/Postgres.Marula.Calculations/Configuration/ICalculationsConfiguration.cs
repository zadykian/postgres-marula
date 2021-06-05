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
		/// Interval of WAL insert location tracking. 
		/// </summary>
		PositiveTimeSpan LsnTrackingInterval();

		/// <summary>
		/// Window in seconds used to calculate average WAL traffic. 
		/// </summary>
		PositiveTimeSpan MovingAverageWindow();
	}
}