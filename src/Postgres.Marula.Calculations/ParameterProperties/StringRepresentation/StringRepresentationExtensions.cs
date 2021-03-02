using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterProperties.StringRepresentation
{
	/// <summary>
	/// Extension method for access to enum items' string representation.
	/// </summary>
	public static class StringRepresentationExtensions
	{
		private static readonly ConcurrentDictionary<
			Type,
			IReadOnlyCollection<(FieldInfo Field, NonEmptyString AsString)>> cache = new();

		/// <summary>
		/// Get string representation of enumeration item <paramref name="enumItem"/>.
		/// </summary>
		/// <remarks>
		/// Each member of type <typeparamref name="TEnum"/> must be configured
		/// with <see cref="StringRepresentationAttribute"/>.
		/// </remarks>
		public static NonEmptyString StringRepresentation<TEnum>(this TEnum enumItem)
			where TEnum : Enum
			=> cache
				.GetOrAdd(typeof(TEnum), MemberNamesToRepresentation)
				.Single(tuple => tuple.Field.Name == enumItem.ToString())
				.AsString;

		/// <summary>
		/// Get enum value by its string representation. 
		/// </summary>
		/// <remarks>
		/// Each member of type <typeparamref name="TEnum"/> must be configured
		/// with <see cref="StringRepresentationAttribute"/>.
		/// </remarks>
		public static TEnum ByStringRepresentation<TEnum>(this NonEmptyString stringRepresentation)
			where TEnum : Enum
			=> (TEnum) cache
				.GetOrAdd(typeof(TEnum), MemberNamesToRepresentation)
				.Single(tuple => tuple.AsString == stringRepresentation)
				.Field
				.GetValue(obj: null)!;

		/// <summary>
		/// Get member names with its string representation from type <paramref name="enumType"/>. 
		/// </summary>
		private static IReadOnlyCollection<(FieldInfo, NonEmptyString)> MemberNamesToRepresentation(IReflect enumType)
			=> enumType
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Select(fieldInfo => (
					fieldInfo,
					fieldInfo.GetCustomAttribute<StringRepresentationAttribute>()!.Value))
				.ToImmutableArray();
	}
}