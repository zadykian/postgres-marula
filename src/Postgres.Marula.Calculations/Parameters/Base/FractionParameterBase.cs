using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <summary>
	/// Fraction database server parameter.
	/// </summary>
	internal abstract class FractionParameterBase : ParameterBase<FractionParameterValue, Fraction>
	{
		protected FractionParameterBase(ILogger<FractionParameterBase> logger) : base(logger)
		{
		}
	}
}