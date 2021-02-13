using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <summary>
	/// Link to database server parameter.
	/// </summary>
	public interface IParameterLink
	{
		/// <summary>
		/// Parameter name.
		/// </summary>
		NonEmptyString Name { get; }
	}

	/// <inheritdoc cref="IParameterLink"/>
	public sealed record ParameterLink(NonEmptyString Name) : IParameterLink;
}