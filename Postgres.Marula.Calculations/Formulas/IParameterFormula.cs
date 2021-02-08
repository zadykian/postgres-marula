using Postgres.Marula.Calculations.Parameters;

namespace Postgres.Marula.Calculations.Formulas
{
	internal interface IParameterFormula
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
}