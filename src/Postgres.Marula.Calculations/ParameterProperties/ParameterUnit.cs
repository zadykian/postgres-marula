using System;

namespace Postgres.Marula.Calculations.ParameterProperties
{
	/// <summary>
	/// Unit of database server parameter.
	/// </summary>
	public enum ParameterUnit : byte
	{
		/// <summary>
		/// Milliseconds (interval, timeout and so on).
		/// </summary>
		[StringRepresentation("ms")]
		Milliseconds = 1,

		/// <summary>
		/// Bytes (RAM, disk).
		/// </summary>
		[StringRepresentation("bytes")]
		Bytes = 2,

		/// <summary>
		/// Enumeration item.
		/// </summary>
		[StringRepresentation("enum")]
		Enum = 3,

		/// <summary>
		/// Without unit (for example, factor).
		/// </summary>
		[StringRepresentation("none")]
		None = 4
	}

	/// <summary>
	/// Extension methods for <see cref="ParameterUnit"/> type.
	/// </summary>
	public static class ParameterUnitExtensions
	{
		/// <summary>
		/// Get number suffix of parameter unit <paramref name="parameterUnit"/>.
		/// </summary>
		public static string NumberSuffix(this ParameterUnit parameterUnit) => parameterUnit switch
		{
			ParameterUnit.Milliseconds => "ms",
			ParameterUnit.Bytes        => "B",
			ParameterUnit.Enum         => string.Empty,
			ParameterUnit.None         => string.Empty,
			_ => throw new ArgumentOutOfRangeException(nameof(parameterUnit), parameterUnit, message: null)
		};
	}
}