using System.Collections.Generic;
using System.Collections.Immutable;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Values;

namespace Postgres.Marula.Calculations.Pipeline
{
	internal readonly struct ParameterContainer
	{
		public ParameterContainer(IParameterValue value)
		{
			Value = value;
		}

		public IParameterValue Value { get; }
	}

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