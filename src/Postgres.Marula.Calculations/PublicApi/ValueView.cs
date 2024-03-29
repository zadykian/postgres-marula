using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.PublicApi
{
	/// <summary>
	/// View of calculated parameter value.
	/// </summary>
	public interface IValueView
	{
		/// <summary>
		/// Link to parameter.
		/// </summary>
		IParameterLink Link { get; }

		/// <summary>
		/// String representation of actual value.
		/// </summary>
		NonEmptyString Value { get; }
	}

	/// <inheritdoc cref="IValueView" />
	public record ValueView(IParameterLink Link, NonEmptyString Value) : IValueView;

	/// <summary>
	/// Extension methods for <see cref="IValueView"/> type.
	/// </summary>
	public static class ValueViewExtensions
	{
		/// <summary>
		/// Represent <paramref name="valueView"/> as ALTER SYSTEM command. 
		/// </summary>
		public static NonEmptyString AsAlterSystem(this IValueView valueView)
			=> $"alter system set {valueView.Link.Name,-36} = '{valueView.Value}';";
	}
}