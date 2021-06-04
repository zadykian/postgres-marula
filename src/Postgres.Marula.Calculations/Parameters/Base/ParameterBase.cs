using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Exceptions;
using Postgres.Marula.Calculations.Parameters.Base.Dependencies;
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
		private readonly AsyncLazy<IParameterValue> valueAsyncCache;

		protected ParameterBase(ILogger<ParameterBase<TParameterValue, TValue>> logger)
		{
			this.logger = logger;
			valueAsyncCache = new(CalculateInternalAsync);
		}

		/// <inheritdoc />
		NonEmptyString IParameterLink.Name => new ParameterLink(GetType()).Name;

		/// <inheritdoc />
		public virtual IParameterDependencies Dependencies() => ParameterDependencies.Empty;

		/// <inheritdoc />
		Task<IParameterValue> IParameter.CalculateAsync() => valueAsyncCache.Value;

		/// <inheritdoc cref="IParameter.CalculateAsync"/>
		/// <remarks>
		/// This implementation calls <see cref="ParameterValueBase{T}(IParameterLink, T)"/> constructor.
		/// </remarks>
		private async Task<IParameterValue> CalculateInternalAsync()
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