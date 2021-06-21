using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Numeric value in range [0.0 .. 1.0].
	/// </summary>
	public readonly struct Fraction : IEquatable<Fraction>, IFormattable
	{
		private readonly decimal underlyingValue;

		public Fraction(decimal underlyingValue)
			=> this.underlyingValue = underlyingValue.InRangeBetween(decimal.Zero, decimal.One)
				? underlyingValue
				: throw new ArgumentException("Value must be in range [0.0 .. 1.0].", nameof(underlyingValue));

		/// <inheritdoc />
		public override string ToString() => underlyingValue.ToString(CultureInfo.InvariantCulture);

		/// <inheritdoc />
		public string ToString(string? format, IFormatProvider? formatProvider) => underlyingValue.ToString(format, formatProvider);

		/// <summary>
		/// Try parse <paramref name="input"/> to <see cref="Fraction"/> instance. 
		/// </summary>
		public static bool TryParse(
			NonEmptyString input,
			[NotNullWhen(returnValue: true)] out Fraction? fraction)
		{
			if (!decimal.TryParse(input, out var decimalValue)
				|| !decimalValue.InRangeBetween(decimal.Zero, decimal.One))
			{
				fraction = null;
				return false;
			}

			fraction = new(decimalValue);
			return true;
		}

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(Fraction other) => underlyingValue == other.underlyingValue;

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is Fraction other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode() => underlyingValue.GetHashCode();

		#endregion

		/// <summary>
		/// Implicit cast operator <see cref="decimal"/> -> <see cref="Fraction"/>.
		/// </summary>
		public static implicit operator Fraction(decimal fraction) => new(fraction);

		/// <summary>
		/// Implicit cast operator <see cref="Fraction"/> -> <see cref="decimal"/>.
		/// </summary>
		public static implicit operator decimal(Fraction fraction) => fraction.underlyingValue;
	}
}