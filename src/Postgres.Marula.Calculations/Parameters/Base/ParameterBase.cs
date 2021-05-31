using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Exceptions;
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
		private readonly ILogger<ParameterBase<TParameterValue, TValue>> logger;

		protected ParameterBase(ILogger<ParameterBase<TParameterValue, TValue>> logger) => this.logger = logger;

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
		{
			TValue parameterValue;

			try
			{
				parameterValue = await CalculateValueAsync();
			}
			catch (ParameterValueCalculationException exception)
			{
				logger.LogError("Failed to calculate parameter value.", exception);
				return NullValue.Instance;
			}

			return Activator
				.CreateInstance(typeof(TParameterValue), this.GetLink(), parameterValue)
				.To(instance => (IParameterValue) instance!);
		}

		/// <summary>
		/// Calculate parameter value. 
		/// </summary>
		protected abstract ValueTask<TValue> CalculateValueAsync();
	}
}