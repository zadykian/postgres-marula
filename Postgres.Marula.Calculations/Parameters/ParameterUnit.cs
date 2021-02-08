namespace Postgres.Marula.Calculations.Parameters
{
	/// <summary>
	/// Unit of database server parameter.
	/// </summary>
	internal enum ParameterUnit : byte
	{
		/// <summary>
		/// Milliseconds (interval, timeout and so on).
		/// </summary>
		Milliseconds = 1,

		/// <summary>
		/// Bytes (RAM, disk).
		/// </summary>
		Bytes = 2,

		/// <summary>
		/// Enumeration item.
		/// </summary>
		EnumItem = 3,

		/// <summary>
		/// Without unit (for example, factor).
		/// </summary>
		None = 4
	}
}