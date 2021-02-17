using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ParameterValues.Base;

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
	}
}