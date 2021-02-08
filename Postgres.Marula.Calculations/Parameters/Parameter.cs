using Postgres.Marula.Calculations.Formulas;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters
{
	/// <summary>
	/// Database server parameter.
	/// </summary>
	internal readonly struct Parameter
	{
		public Parameter(NonEmptyString name, ParameterUnit unit)
		{
			Name = name;
			Unit = unit;
		}

		/// <summary>
		/// Parameter name.
		/// </summary>
		public NonEmptyString Name { get; }
		
		/// <summary>
		/// Parameter unit.
		/// </summary>
		public ParameterUnit Unit { get; }
	}
}