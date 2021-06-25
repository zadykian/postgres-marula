using System.Collections.Generic;

namespace Postgres.Marula.Calculations.PublicApi
{
	/// <summary>
	/// Calculated parameter values.
	/// </summary>
	public interface IParameterValues
	{
		/// <summary>
		/// Get parameter values calculated during most recent job iteration. 
		/// </summary>
		IAsyncEnumerable<IValueView> MostRecentAsync();
	}
}