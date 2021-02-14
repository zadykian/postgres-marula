using System;
using System.Globalization;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Numeric value in range [0.0 .. 1.0].
	/// </summary>
	public readonly struct Fraction : IEquatable<Fraction>, IFormattable
	{
		private readonly decimal fractionValue;

		public Fraction(decimal fractionValue)
			=> this.fractionValue = fractionValue.InRangeBetween(decimal.Zero, decimal.One)
				? fractionValue
				: throw new ArgumentException("Value must be in range [0.0 .. 1.0].", nameof(fractionValue));

		/// <inheritdoc />
		public override string ToString() => fractionValue.ToString(CultureInfo.InvariantCulture);

		/// <inheritdoc />
		public string ToString(string? format, IFormatProvider? formatProvider) => fractionValue.ToString(format, formatProvider);

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(Fraction other) => fractionValue == other.fractionValue;

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is Fraction other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode() => fractionValue.GetHashCode();

		/// <summary>
		/// <see cref="Fraction"/> equality operator. 
		/// </summary>
		public static bool operator ==(Fraction left, Fraction right) => left.Equals(right);

		/// <inheritdoc cref="op_Equality"/>
		public static bool operator !=(Fraction left, Fraction right) => !left.Equals(right);

		#endregion

		/// <summary>
		/// Implicit cast operator <see cref="decimal"/> -> <see cref="Fraction"/>. 
		/// </summary>
		public static implicit operator Fraction(decimal fraction) => new (fraction);

		/// <summary>
		/// Implicit cast operator <see cref="Fraction"/> -> <see cref="decimal"/>.
		/// </summary>
		public static implicit operator decimal(Fraction fraction) => fraction.fractionValue;
	}
}