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
	}
}