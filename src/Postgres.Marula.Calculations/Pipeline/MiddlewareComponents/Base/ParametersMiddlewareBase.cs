using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.ParameterValues.Base;

namespace Postgres.Marula.Calculations.Pipeline.MiddlewareComponents.Base
{
	/// <summary>
	/// Pipeline component base type.
	/// </summary>
	internal abstract class ParametersMiddlewareBase
	{
		private readonly ICalculationsConfiguration calculationsConfiguration;

		protected ParametersMiddlewareBase(ICalculationsConfiguration calculationsConfiguration)
			=> this.calculationsConfiguration = calculationsConfiguration;

		/// <summary>
		/// Check if parameter value <paramref name="parameterValue"/>
		/// can be applied to database server.
		/// </summary>
		protected bool ParameterAdjustmentIsAllowed(IParameterValue parameterValue)
			=> calculationsConfiguration.AutoAdjustmentIsEnabled();
	}
}