namespace Postgres.Marula.Calculations.ParameterValues.Raw
{
	/// <summary>
	/// Type of raw parameter value.
	/// </summary>
	/// <remarks>
	/// Type is being retrieved from 'pg_catalog.pg_settings.vartype' column.
	/// </remarks>
	public enum RawValueType : byte
	{
		/// <summary>
		/// Boolean type.
		/// </summary>
		Bool,

		/// <summary>
		/// Enumeration type.
		/// </summary>
		Enum,

		/// <summary>
		/// Integer type.
		/// </summary>
		Integer,

		/// <summary>
		/// Floating point numeric type.
		/// </summary>
		Real,

		/// <summary>
		/// String type.
		/// </summary>
		String
	}
}