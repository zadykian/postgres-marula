using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
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
		public override ParameterUnit Unit => ParameterUnit.Bytes;

		/// <inheritdoc />
		public override NonEmptyString AsString() => Value.TotalBytes.ToString();
	}
}