using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterValues
{
	/// <summary>
	/// Database memory parameter value represented in bytes.
	/// </summary>
	public class MemoryParameterValue : ParameterValueBase<Memory>
	{
		public MemoryParameterValue(IParameterLink parameterLink, Memory value)
			: base(parameterLink, value)
		{
		}

		/// <inheritdoc />
		public override IUnit Unit => new IUnit.Mem(Value.Normalized().Unit);

		// /// <inheritdoc />
		// public override NonEmptyString AsString() => Value.TotalBytes.ToString();
	}
}