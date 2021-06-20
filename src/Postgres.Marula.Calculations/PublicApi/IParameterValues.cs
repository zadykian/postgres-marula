using System.Collections.Generic;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.PublicApi
{
	/// <summary>
	/// Calculated parameter values.
	/// </summary>
	public interface IParameterValues
	{
		/// <summary>
		/// Get parameter values calculated during most recent job iteration. 
		/// </summary>
		IAsyncEnumerable<IValueView> MostRecentAsync();
	}

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
}