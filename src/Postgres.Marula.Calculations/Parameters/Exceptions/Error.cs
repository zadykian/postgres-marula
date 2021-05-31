using Postgres.Marula.Calculations.Configuration;

namespace Postgres.Marula.Calculations.Parameters.Exceptions
{
	/// <summary>
	/// Component which represents errors related to parameters calculations.
	/// </summary>
	internal static class Error
	{
		/// <summary>
		/// Get exception object which is thrown when
		/// there are no any LSN history entries
		/// in window <see cref="ICalculationsConfiguration.MovingAverageWindow"/>.
		/// </summary>
		public static ParameterValueCalculationException NoLsnHistory()
			=> new("There are no any LSN history entries in configured window.");
	}
}