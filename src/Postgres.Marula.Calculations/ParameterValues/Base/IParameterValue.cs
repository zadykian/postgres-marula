using System;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;

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
		IUnit Unit { get; }
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
		IParameterLink IParameterValue.ParameterLink => throw AccessError();

		/// <inheritdoc />
		IUnit IParameterValue.Unit => throw AccessError();

		/// <summary>
		/// Get exception object to throw on any member access. 
		/// </summary>
		private static InvalidOperationException AccessError() => throw new($"Unable to access {nameof(NullValue)} members.");
	}
}