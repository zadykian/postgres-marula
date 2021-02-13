using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.Infrastructure.Configuration;

namespace Postgres.Marula.Calculations.Pipeline.MiddlewareComponents.Base
{
	/// <summary>
	/// Pipeline component base type.
	/// </summary>
	internal abstract class ParametersMiddlewareBase
	{
		private readonly IAppConfiguration appConfiguration;

		protected ParametersMiddlewareBase(IAppConfiguration appConfiguration) => this.appConfiguration = appConfiguration;
		
		/// <summary>
		/// Check if parameter value <paramref name="parameterValue"/>
		/// can be applied to database server.
		/// </summary>
		protected bool ParameterAdjustmentIsAllowed(IParameterValue parameterValue) => appConfiguration.AutoAdjustIsEnabled();
	}
}