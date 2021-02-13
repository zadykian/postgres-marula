using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.Parameters.Values.Base;

namespace Postgres.Marula.Calculations.Parameters.Values
{
	/// <summary>
	/// Database memory parameter value represented in bytes.
	/// </summary>
	public class MemoryParameterValue : ParameterValueBase<ulong>
	{
		public MemoryParameterValue(IParameterLink parameterLink, ulong value) : base(parameterLink, value)
		{
		}

		/// <inheritdoc />
		public override ParameterUnit Unit => ParameterUnit.Bytes;
	}
}