using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <summary>
	/// Database server parameter.
	/// </summary>
	internal interface IParameter : IParameterLink
	{
		/// <summary>
		/// Parameter context. 
		/// </summary>
		ParameterContext Context { get; }

		/// <summary>
		/// Calculate parameter value. 
		/// </summary>
		IParameterValue Calculate();
	}

	/// <summary>
	/// Extension methods for <see cref="IParameter"/> type.
	/// </summary>
	internal static class ParameterExtensions
	{
		/// <inheritdoc cref="IParameterLink"/>
		private sealed record ParameterLink(NonEmptyString Name) : IParameterLink;

		/// <summary>
		/// Get link to database parameter <paramref name="parameter"/>. 
		/// </summary>
		public static IParameterLink GetLink(this IParameter parameter) => new ParameterLink(parameter.Name);
	}
}