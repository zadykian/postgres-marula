using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterValues.Base
{
	/// <summary>
	/// Calculated parameter value.
	/// </summary>
	public interface IParameterValue
	{
		/// <summary>
		/// Link to parameter.
		/// </summary>
		IParameterLink ParameterLink { get; }

		/// <summary>
		/// Parameter unit.
		/// </summary>
		ParameterUnit Unit { get; }

		/// <summary>
		/// Represent parameter value as string without unit. 
		/// </summary>
		NonEmptyString AsString();
	}

	/// <summary>
	/// Represents absence of value.
	/// </summary>
	public sealed class NullValue : IParameterValue
	{
		private NullValue()
		{
		}

		/// <inheritdoc cref="NullValue"/>
		public static IParameterValue Instance { get; } = new NullValue();

		/// <inheritdoc />
		IParameterLink IParameterValue.ParameterLink => new ParameterLink(nameof(NullValue));

		/// <inheritdoc />
		ParameterUnit IParameterValue.Unit => ParameterUnit.None;

		/// <inheritdoc />
		NonEmptyString IParameterValue.AsString() => string.Empty;
	}
}