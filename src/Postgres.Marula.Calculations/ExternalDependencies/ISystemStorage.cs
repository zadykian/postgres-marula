using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.Parameters.Values.Base;

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
		Task SaveParameterValuesAsync(IEnumerable<ParameterValueWithStatus> parameterValues);
	}
}