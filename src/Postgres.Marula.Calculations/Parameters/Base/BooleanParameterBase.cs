using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.ParameterValues;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <summary>
	/// Boolean database server parameter.
	/// </summary>
	internal abstract class BooleanParameterBase : ParameterBase<BooleanParameterValue, bool>
	{
		protected BooleanParameterBase(ILogger<BooleanParameterBase> logger) : base(logger)
		{
		}
	}
}