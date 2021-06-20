using Postgres.Marula.Calculations.Parameters.Base;

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
}