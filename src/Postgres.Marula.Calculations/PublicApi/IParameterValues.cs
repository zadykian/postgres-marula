using System.Collections.Generic;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;
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
		IAsyncEnumerable<IValueView> MostRecent();
	}

	/// <summary>
	/// View of calculated parameter value.
	/// </summary>
	public interface IValueView : IHasLinkToParameter
	{
		/// <summary>
		/// String representation of actual value.
		/// </summary>
		NonEmptyString Value { get; }
	}

	/// <inheritdoc cref="IValueView" />
	public record ValueView(IParameterLink Link, NonEmptyString Value) : IValueView;
}