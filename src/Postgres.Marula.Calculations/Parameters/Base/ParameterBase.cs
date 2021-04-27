using System;
using System.Threading.Tasks;
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
		/// <remarks>
		/// This implementation calls <see cref="ParameterValueBase{T}(IParameterLink, T)"/> constructor.
		/// </remarks>
		async ValueTask<IParameterValue> IParameter.CalculateAsync()
			=> Activator
				.CreateInstance(typeof(TParameterValue), this.GetLink(), await CalculateValueAsync())
				.To(instance => (IParameterValue) instance!);

		/// <summary>
		/// Calculate parameter value. 
		/// </summary>
		protected abstract ValueTask<TValue> CalculateValueAsync();
	}
}