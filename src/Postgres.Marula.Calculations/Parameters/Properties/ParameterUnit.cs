using System;

namespace Postgres.Marula.Calculations.Parameters.Properties
{
	/// <summary>
	/// Unit of database server parameter.
	/// </summary>
	public enum ParameterUnit : byte
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
		Enum = 3,

		/// <summary>
		/// Without unit (for example, factor).
		/// </summary>
		None = 4
	}

	/// <summary>
	/// Extension methods for <see cref="ParameterUnit"/> type.
	/// </summary>
	public static class ParameterUnitExtensions
	{
		/// <summary>
		/// Get string representation of parameter unit <paramref name="parameterUnit"/>.
		/// </summary>
		public static string AsString(this ParameterUnit parameterUnit) => parameterUnit switch
		{
			ParameterUnit.Milliseconds => "ms",
			ParameterUnit.Bytes        => "B",
			ParameterUnit.Enum         => string.Empty,
			ParameterUnit.None         => string.Empty,
			_ => throw new ArgumentOutOfRangeException(nameof(parameterUnit), parameterUnit, null)
		};
	}
}