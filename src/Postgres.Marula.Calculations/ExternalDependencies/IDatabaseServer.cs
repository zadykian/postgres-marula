using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.ParameterValues.Raw;
using Postgres.Marula.Infrastructure.TypeDecorators;

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
		Task ApplyToConfigurationAsync(IReadOnlyCollection<IParameterValue> parameterValues);

		/// <summary>
		/// Get raw value of parameter named <paramref name="parameterName"/>.
		/// </summary>
		Task<RawParameterValue> GetRawParameterValueAsync(NonEmptyString parameterName);

		/// <summary>
		/// Get context of parameter named <paramref name="parameterName"/>. 
		/// </summary>
		ValueTask<ParameterContext> GetParameterContextAsync(NonEmptyString parameterName);
	}
}