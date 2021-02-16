using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <inheritdoc />
	internal abstract class ParameterBase : IParameter
	{
		/// <inheritdoc />
		public abstract NonEmptyString Name { get; }

		public ParameterContext Context { get; }

		/// <inheritdoc />
		public abstract IParameterValue Calculate();
	}
}