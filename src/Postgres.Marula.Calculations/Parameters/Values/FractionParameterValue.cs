using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Values
{
	/// <summary>
	/// Database parameter value represented as number in range [0.0 .. 1.0].
	/// </summary>
	public class FractionParameterValue : ParameterValueBase<Fraction>
	{
		public FractionParameterValue(IParameterLink parameterLink, Fraction value)
			: base(parameterLink, value)
		{
		}

		/// <inheritdoc />
		public override ParameterUnit Unit => ParameterUnit.None;

		/// <inheritdoc />
		public override NonEmptyString AsString() => Value.ToString();
	}
}