using System.Collections.Generic;
using System.Collections.Immutable;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;

namespace Postgres.Marula.Calculations.Pipeline
{
	/// <summary>
	/// Parameters pipeline context.
	/// </summary>
	internal record ParametersManagementContext
	{
		public ParametersManagementContext(IEnumerable<IParameter> parameters)
		{
			Parameters = parameters.ToImmutableArray();
			CalculatedValues = ImmutableArray<IParameterValue>.Empty;
		}

		/// <summary>
		/// Parameters being calculated.
		/// </summary>
		public IReadOnlyCollection<IParameter> Parameters { get; }

		/// <summary>
		/// Calculated parameter values. 
		/// </summary>
		public IReadOnlyCollection<IParameterValue> CalculatedValues { get; init; }
	}
}