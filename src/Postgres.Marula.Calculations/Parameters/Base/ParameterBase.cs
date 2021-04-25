using System;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <inheritdoc />
	internal abstract class ParameterBase<TParameterValue, TValue> : IParameter
		where TParameterValue : ParameterValueBase<TValue>
		where TValue : IEquatable<TValue>
	{
		/// <inheritdoc />
		NonEmptyString IParameterLink.Name
			=> GetType()
				.Name
				.ToSnakeCase();

		/// <inheritdoc />
		IParameterValue IParameter.Calculate()
			=> Activator
				.CreateInstance(typeof(TParameterValue), this.GetLink(), CalculateValue())
				.To(instance => (IParameterValue) instance!);

		/// <summary>
		/// Calculate parameter value. 
		/// </summary>
		protected abstract TValue CalculateValue();
	}
}