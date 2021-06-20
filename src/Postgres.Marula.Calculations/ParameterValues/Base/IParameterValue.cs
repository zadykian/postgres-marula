using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterValues.Base
{
	/// <summary>
	/// Something that has link to database server parameter.
	/// </summary>
	public interface IHasLinkToParameter
	{
		/// <summary>
		/// Link to parameter.
		/// </summary>
		IParameterLink Link { get; }
	}

	/// <summary>
	/// Calculated parameter value.
	/// </summary>
	public interface IParameterValue : IHasLinkToParameter
	{
		/// <summary>
		/// Parameter unit.
		/// </summary>
		IUnit Unit { get; }
	}

	/// <summary>
	/// View of calculated parameter value.
	/// </summary>
	public interface IParameterValueView : IHasLinkToParameter
	{
		/// <summary>
		/// String representation of actual value.
		/// </summary>
		NonEmptyString Value { get; }
	}

	
}