using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ExternalDependencies
{
	/// <summary>
	/// Application system storage.
	/// </summary>
	public interface ISystemStorage
	{
		/// <summary>
		/// Save calculated parameter values.
		/// </summary>
		Task SaveParameterValuesAsync(IReadOnlyCollection<ParameterValueWithStatus> parameterValues);

		/// <summary>
		/// Save current LSN value.
		/// </summary>
		Task SaveLogSeqNumberAsync(LogSeqNumber logSeqNumber);

		/// <summary>
		/// Get most resent LSN values which were saved not earlier
		/// then current time minus <paramref name="window"/>. 
		/// </summary>
		IAsyncEnumerable<LsnHistoryEntry> GetLsnHistoryAsync(PositiveTimeSpan window);

		/// <summary>
		/// Save current average bloat fraction. 
		/// </summary>
		Task SaveBloatFractionAsync(Fraction averageBloatFraction);

		/// <summary>
		/// Get most resent bloat fraction values which were saved not earlier
		/// then current time minus <paramref name="window"/>. 
		/// </summary>
		IAsyncEnumerable<BloatFractionHistoryEntry> GetBloatFractionHistory(PositiveTimeSpan window);
	}
}