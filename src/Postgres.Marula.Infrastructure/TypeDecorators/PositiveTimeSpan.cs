using System;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Positive time interval.
	/// </summary>
	public readonly struct PositiveTimeSpan : IEquatable<PositiveTimeSpan>, IFormattable
	{
		private readonly TimeSpan underlyingValue;

		private PositiveTimeSpan(TimeSpan underlyingValue)
			=> this.underlyingValue = underlyingValue <= TimeSpan.Zero
				? throw new ArgumentException("Timespan value must be positive.", nameof(underlyingValue))
				: underlyingValue;

		/// <inheritdoc cref="TimeSpan.TotalMilliseconds"/>
		public double TotalMilliseconds => underlyingValue.TotalMilliseconds;

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
		/// Implicit cast operator <see cref="TimeSpan"/> -> <see cref="PositiveTimeSpan"/>. 
		/// </summary>
		public static implicit operator PositiveTimeSpan(TimeSpan timeSpan) => new(timeSpan);

		/// <summary>
		/// Implicit cast operator <see cref="PositiveTimeSpan"/> -> <see cref="TimeSpan"/>. 
		/// </summary>
		public static implicit operator TimeSpan(PositiveTimeSpan positiveTimeSpan) => positiveTimeSpan.underlyingValue;
	}
}