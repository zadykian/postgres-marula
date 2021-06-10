using System;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;

namespace Postgres.Marula.Calculations.ParameterValues.Base
{
	/// <inheritdoc cref="IParameterValue"/>
	public abstract class ParameterValueBase<T> : IParameterValue, IEquatable<ParameterValueBase<T>>
		where T : IEquatable<T>
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
		public abstract IUnit Unit { get; }

		/// <inheritdoc />
		public override string ToString() => Value.ToString()!;

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(ParameterValueBase<T>? other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Value.Equals(other.Value) && ParameterLink.Equals(other.ParameterLink) && Unit.Equals(other.Unit);
		}

		/// <inheritdoc />
		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((ParameterValueBase<T>) obj);
		}

		/// <inheritdoc />
		public override int GetHashCode() => HashCode.Combine(Value, ParameterLink, Unit);

		#endregion
	}
}