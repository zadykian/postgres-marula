using System;
using System.Collections.Generic;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Values.Base
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
		public abstract ParameterUnit Unit { get; }

		/// <inheritdoc />
		public abstract NonEmptyString AsString();

		/// <inheritdoc />
		public override string ToString() => $"{ParameterLink.Name}: {AsString()}{Unit.AsString()}";

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(ParameterValueBase<T>? other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return EqualityComparer<T>.Default.Equals(Value, other.Value) && ParameterLink.Equals(other.ParameterLink) && Unit == other.Unit;
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
		public override int GetHashCode() => HashCode.Combine(Value, ParameterLink, (int) Unit);

		/// <summary>
		/// <see cref="ParameterValueBase{T}"/> equality operator. 
		/// </summary>
		public static bool operator ==(ParameterValueBase<T>? left, ParameterValueBase<T>? right) => Equals(left, right);

		/// <see cref="op_Equality"/>
		public static bool operator !=(ParameterValueBase<T>? left, ParameterValueBase<T>? right) => !Equals(left, right);

		#endregion
	}
}