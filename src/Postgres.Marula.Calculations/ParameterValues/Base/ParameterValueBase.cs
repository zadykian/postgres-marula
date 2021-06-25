using System;
using System.Diagnostics.CodeAnalysis;
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
			Link = parameterLink;
			Value = value;
		}

		/// <summary>
		/// Value or parameter.
		/// </summary>
		public T Value { get; }

		/// <inheritdoc />
		public IParameterLink Link { get; }

		/// <inheritdoc />
		public abstract IUnit Unit { get; }

		/// <inheritdoc />
		[return: NotNull]
		public sealed override string ToString() => Value.ToString()!;

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(ParameterValueBase<T>? other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Value.Equals(other.Value) && Link.Equals(other.Link) && Unit.Equals(other.Unit);
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
		public override int GetHashCode() => HashCode.Combine(Value, Link, Unit);

		#endregion
	}
}