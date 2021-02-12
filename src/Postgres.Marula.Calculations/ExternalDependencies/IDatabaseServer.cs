using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.Parameters.Values;

namespace Postgres.Marula.Calculations.ExternalDependencies
{
	/// <summary>
	/// Database server being tuned.
	/// </summary>
	public interface IDatabaseServer
	{
		/// <summary>
		/// Apply parameter values <paramref name="parameterValues"/>
		/// to database server configuration.
		/// </summary>
		Task ApplyToConfigurationAsync(IEnumerable<IParameterValue> parameterValues);
	}
}