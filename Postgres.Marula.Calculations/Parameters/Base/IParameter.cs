using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <summary>
	/// Database server parameter.
	/// </summary>
	internal interface IParameter<out TValue>
		where TValue : IParameterValue
	{
		/// <summary>
		/// Parameter name.
		/// </summary>
		NonEmptyString Name { get; }

		/// <summary>
		/// Calculate parameter value. 
		/// </summary>
		TValue Calculate();
	}
}