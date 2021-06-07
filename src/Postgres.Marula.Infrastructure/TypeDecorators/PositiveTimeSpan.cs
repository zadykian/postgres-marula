using System;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Positive time interval.
	/// </summary>
	public readonly struct PositiveTimeSpan : IEquatable<PositiveTimeSpan>, IFormattable
	{
		private readonly TimeSpan underlyingValue;

		public PositiveTimeSpan(TimeSpan underlyingValue)
			=> this.underlyingValue = underlyingValue <= TimeSpan.Zero
				? throw new ArgumentException("Timespan value must be positive.", nameof(underlyingValue))
				: underlyingValue;

		/// <inheritdoc cref="TimeSpan.TotalMilliseconds"/>
		public double TotalMilliseconds => underlyingValue.TotalMilliseconds;

		/// <inheritdoc cref="TimeSpan.TotalSeconds"/>
		public double TotalSeconds => underlyingValue.TotalSeconds;

		/// <inheritdoc />
		public override string ToString() => underlyingValue.ToString();

		/// <inheritdoc />
		public string ToString(string? format, IFormatProvider? formatProvider) => underlyingValue.ToString(format, formatProvider);

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(PositiveTimeSpan other) => underlyingValue.Equals(other.underlyingValue);

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is PositiveTimeSpan other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode() => underlyingValue.GetHashCode();

		#endregion

		/// <summary>
		/// Convert string <paramref name="stringToParse"/> to timespan value.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Occurs when <paramref name="stringToParse"/> has invalid format.
		/// </exception>
		public static PositiveTimeSpan Parse(NonEmptyString stringToParse)
		{
			var (totalMilliseconds, unit) = stringToParse.ParseToTokens();

			var multiplier = unit switch
			{
				"ms"  => 1,
				"s"   => 1000,
				"min" => 60 * 1000,
				"h"   => 60 * 60 * 1000,
				_     => throw new ArgumentOutOfRangeException(nameof(stringToParse), stringToParse, message: null)
			};

			return TimeSpan.FromMilliseconds(totalMilliseconds * (ulong) multiplier);
		}

		/// <summary>
		/// Multiplication operator. 
		/// </summary>
		public static PositiveTimeSpan operator *(PositiveTimeSpan timeSpan, double coefficient)
			=> TimeSpan.FromMilliseconds(timeSpan.TotalMilliseconds * coefficient);

		/// <summary>
		/// Implicit cast operator <see cref="TimeSpan"/> -> <see cref="PositiveTimeSpan"/>.
		/// </summary>
		public static implicit operator PositiveTimeSpan(TimeSpan timeSpan) => new(timeSpan);

		/// <summary>
		/// Implicit cast operator <see cref="PositiveTimeSpan"/> -> <see cref="TimeSpan"/>.
		/// </summary>
		public static implicit operator TimeSpan(PositiveTimeSpan positiveTimeSpan) => positiveTimeSpan.underlyingValue;
	}
}