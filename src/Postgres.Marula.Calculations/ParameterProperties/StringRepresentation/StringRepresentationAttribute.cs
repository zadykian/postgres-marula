using System;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterProperties.StringRepresentation
{
	/// <summary>
	/// Attribute to configure string representation for class member.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class StringRepresentationAttribute : Attribute
	{
		internal StringRepresentationAttribute(string value) => Value = value;

		/// <summary>
		/// String representation.
		/// </summary>
		public NonEmptyString Value { get; }
	}
}