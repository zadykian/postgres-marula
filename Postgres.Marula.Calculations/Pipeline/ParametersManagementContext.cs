using System.Collections.Generic;
using Postgres.Marula.Calculations.Parameters.Base;

namespace Postgres.Marula.Calculations.Pipeline
{
	internal class ParametersManagementContext
	{
		public ParametersManagementContext(IReadOnlyCollection<IParameter> parameters) => Parameters = parameters;

		public IReadOnlyCollection<IParameter> Parameters { get; }
	}
}