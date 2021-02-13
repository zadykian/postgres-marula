using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;

namespace Postgres.Marula.Calculations.Parameters.Values.Base
{
	/// <inheritdoc />
	public abstract class ParameterValueBase<T> : IParameterValue
		where T : notnull
	{
		private protected ParameterValueBase(IParameterLink parameterLink, T value)
		{
			ParameterLink = parameterLink;
			Value = value;
		}

		/// <summary>
		/// Value or parameter.
		/// </summary>
		public T Value { get; }

		/// <inheritdoc />
		public IParameterLink ParameterLink { get; }

		/// <inheritdoc />
		public abstract ParameterUnit Unit { get; }

		/// <inheritdoc />
		public virtual string AsStringValue() => $"{Value.ToString()}{Unit.AsString()}";
	}
}