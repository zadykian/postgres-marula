using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Values
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

		public MemoryParameterValue(IParameterLink parameterLink, ulong totalBytes)
			: this(parameterLink, new Memory(totalBytes))
		{
		}

		/// <inheritdoc />
		public override ParameterUnit Unit => ParameterUnit.Bytes;
	}
}