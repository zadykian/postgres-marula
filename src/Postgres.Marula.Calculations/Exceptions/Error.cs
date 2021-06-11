using Postgres.Marula.Calculations.Configuration;

namespace Postgres.Marula.Calculations.Exceptions
{
	/// <summary>
	/// Component which represents errors related to parameters calculations.
	/// </summary>
	internal static class Error
	{
		/// <summary>
		/// Get exception object which is thrown when
		/// there are not enough LSN history entries
		/// in window <see cref="IPeriodicLoggingConfiguration.MovingAverageWindow"/>.
		/// </summary>
		public static ParameterValueCalculationException NoLsnHistory()
			=> new("There are not enough LSN history entries in configured window.");

		/// <summary>
		/// Get exception object which is thrown when
		/// there not enough bloat fraction history entries
		/// in window <see cref="IPeriodicLoggingConfiguration.MovingAverageWindow"/>.
		/// </summary>
		public static ParameterValueCalculationException NoBloatHistory()
			=> new("There are not enough bloat fraction history entries in configured window.");
	}
}