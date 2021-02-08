using System;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Positive time interval.
	/// </summary>
	public readonly struct PositiveTimespan : IEquatable<PositiveTimespan>, IFormattable
	{
		private readonly TimeSpan underlyingValue;

		private PositiveTimespan(TimeSpan underlyingValue)
			=> this.underlyingValue = underlyingValue <= TimeSpan.Zero
				? throw new ArgumentException("Timespan value must be positive.", nameof(underlyingValue))
				: underlyingValue;

		/// <inheritdoc />
		public override string ToString() => underlyingValue.ToString();

		/// <inheritdoc />
		public string ToString(string? format, IFormatProvider? formatProvider) => underlyingValue.ToString(format, formatProvider);

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(PositiveTimespan other) => underlyingValue.Equals(other.underlyingValue);

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is PositiveTimespan other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode() => underlyingValue.GetHashCode();

		/// <summary>
		/// <see cref="PositiveTimespan"/> equality operator. 
		/// </summary>
		public static bool operator ==(PositiveTimespan left, PositiveTimespan right) => left.Equals(right);

		/// <see cref="op_Equality"/>
		public static bool operator !=(PositiveTimespan left, PositiveTimespan right) => !left.Equals(right);

		#endregion

		/// <summary>
		/// Implicit cast operator <see cref="TimeSpan"/> -> <see cref="PositiveTimespan"/>. 
		/// </summary>
		public static implicit operator PositiveTimespan(TimeSpan timeSpan) => new (timeSpan);

		/// <summary>
		/// Implicit cast operator <see cref="PositiveTimespan"/> -> <see cref="TimeSpan"/>. 
		/// </summary>
		public static implicit operator TimeSpan(PositiveTimespan positiveTimespan) => positiveTimespan.underlyingValue;
	}
}