using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.ParameterValues;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <summary>
	/// Integer database server parameter.
	/// </summary>
	internal abstract class IntegerParameterBase : ParameterBase<IntegerParameterValue, uint>
	{
		protected IntegerParameterBase(ILogger<IntegerParameterBase> logger) : base(logger)
		{
		}
	}
}