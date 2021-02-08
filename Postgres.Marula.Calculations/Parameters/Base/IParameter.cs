using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <summary>
	/// Database server parameter.
	/// </summary>
	internal interface IParameter
	{
		/// <summary>
		/// Parameter name.
		/// </summary>
		NonEmptyString Name { get; }

		/// <summary>
		/// Parameter unit.
		/// </summary>
		ParameterUnit Unit { get; }

		/// <summary>
		/// Calculate parameter value. 
		/// </summary>
		CalculationResult Calculate();
	}
}