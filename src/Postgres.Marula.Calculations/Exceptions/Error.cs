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
		/// there is not enough LSN history entries
		/// in window <see cref="IPeriodicLoggingConfiguration.MovingAverageWindow"/>.
		/// </summary>
		public static ParameterValueCalculationException NoLsnHistory()
			=> new("There is not enough LSN history entries in configured window.");

		/// <summary>
		/// Get exception object which is thrown when
		/// there is not enough bloat fraction history entries
		/// in window <see cref="IPeriodicLoggingConfiguration.MovingAverageWindow"/>.
		/// </summary>
		public static ParameterValueCalculationException NoBloatHistory()
			=> new("There is not enough bloat fraction history entries in configured window.");
	}
}