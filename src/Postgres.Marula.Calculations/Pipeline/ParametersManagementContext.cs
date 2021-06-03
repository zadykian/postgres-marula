using System.Collections.Generic;
using System.Collections.Immutable;
using Postgres.Marula.Calculations.Parameters.Base;

namespace Postgres.Marula.Calculations.Pipeline
{
	/// <summary>
	/// Parameters pipeline context.
	/// </summary>
	internal record ParametersManagementContext
	{
		public ParametersManagementContext(IEnumerable<IParameter> parameters)
			=> Parameters = parameters.ToImmutableArray();

		/// <summary>
		/// Parameters being calculated.
		/// </summary>
		public IReadOnlyCollection<IParameter> Parameters { get; }
	}
}