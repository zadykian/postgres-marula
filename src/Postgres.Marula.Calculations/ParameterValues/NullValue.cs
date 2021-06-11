using System;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;

namespace Postgres.Marula.Calculations.ParameterValues
{
	/// <summary>
	/// Represents absence of value.
	/// </summary>
	public sealed class NullValue : IParameterValue
	{
		private NullValue(IParameterLink link) => Link = link;

		/// <summary>
		/// Create new instance of <see cref="NullValue"/> related to <paramref name="parameter"/>. 
		/// </summary>
		public static IParameterValue OfParameter(IParameter parameter)
			=> new NullValue(parameter.GetLink());

		/// <inheritdoc />
		public IParameterLink Link { get; }

		/// <inheritdoc />
		IUnit IParameterValue.Unit => throw AccessError();

		/// <summary>
		/// Get exception object to throw on any member access. 
		/// </summary>
		private static InvalidOperationException AccessError() => throw new($"Unable to access {nameof(NullValue)} members.");
	}
}