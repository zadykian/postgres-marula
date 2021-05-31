using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <summary>
	/// Timespan database server parameter.
	/// </summary>
	internal abstract class TimeSpanParameterBase : ParameterBase<TimeSpanParameterValue, PositiveTimeSpan>
	{
		protected TimeSpanParameterBase(ILogger<TimeSpanParameterBase> logger) : base(logger)
		{
		}
	}
}