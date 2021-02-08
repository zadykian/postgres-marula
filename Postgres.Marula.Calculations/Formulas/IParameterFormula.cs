using Postgres.Marula.Calculations.Parameters;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Formulas
{
	internal interface IParameterFormula<out TValue>
	{
		/// <summary>
		/// Parameter being calculated.
		/// </summary>
		Parameter Parameter { get; }

		/// <summary>
		/// Calculate parameter value. 
		/// </summary>
		CalculationResult CalculateValue();
	}

	internal readonly struct CalculationResult
	{
		
	}
}