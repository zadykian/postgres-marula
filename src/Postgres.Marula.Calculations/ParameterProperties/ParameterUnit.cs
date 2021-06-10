using Postgres.Marula.Calculations.ParameterProperties.StringRepresentation;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterProperties
{
	/// <summary>
	/// Unit of database server parameter.
	/// </summary>
	public interface IUnit
	{
		/// <summary>
		/// Memory (RAM, disk).
		/// </summary>
		record Mem(Memory.Unit Unit) : IUnit;

		/// <summary>
		/// Milliseconds (interval, timeout and so on).
		/// </summary>
		record Milliseconds : IUnit;

		/// <summary>
		/// Enumeration item.
		/// </summary>
		record Enum : IUnit;

		/// <summary>
		/// Without unit (fraction, integer, etc).
		/// </summary>
		record None : IUnit;
	}

	public static class UnitExtensions
	{
		public static NonEmptyString AsString(this IUnit unit)
			=> unit switch
			{
				IUnit.Mem mem => mem.Unit.StringRepresentation(),
				_ => unit.GetType().Name.ToSnakeCase()
			};
	}

	// /// <summary>
	// /// Unit of database server parameter.
	// /// </summary>
	// public enum ParameterUnit : byte
	// {
	// 	/// <summary>
	// 	/// Milliseconds (interval, timeout and so on).
	// 	/// </summary>
	// 	Milliseconds = 1,
	//
	// 	/// <summary>
	// 	/// Bytes (RAM, disk).
	// 	/// </summary>
	// 	Bytes = 2,
	//
	// 	/// <summary>
	// 	/// Enumeration item.
	// 	/// </summary>
	// 	Enum = 3,
	//
	// 	/// <summary>
	// 	/// Without unit (fraction, integer, etc).
	// 	/// </summary>
	// 	None = 4
	// }
}