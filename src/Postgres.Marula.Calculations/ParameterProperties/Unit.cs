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

	/// <summary>
	/// Extension methods for <see cref="IUnit"/> type.
	/// </summary>
	public static class UnitExtensions
	{
		/// <summary>
		/// Get string representation of <paramref name="unit"/>. 
		/// </summary>
		public static NonEmptyString AsString(this IUnit unit)
			=> unit switch
			{
				IUnit.Mem mem => mem.Unit.StringRepresentation(),
				_ => unit.GetType().Name.ToSnakeCase()
			};
	}
}