using System;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Non-empty string.
	/// </summary>
	public readonly struct NonEmptyString : IEquatable<NonEmptyString>
	{
		private readonly string underlyingValue;

		private NonEmptyString(string underlyingValue)
			=> this.underlyingValue = string.IsNullOrWhiteSpace(underlyingValue)
				? throw new ArgumentException("String value can't be whitespace.", nameof(underlyingValue))
				: underlyingValue;

		/// <summary>
		/// Replace substring <paramref name="substring"/> with value <paramref name="replaceWith"/>
		/// and return new <see cref="NonEmptyString"/> instance. 
		/// </summary>
		public NonEmptyString Replace(NonEmptyString substring, string replaceWith)=> underlyingValue.Replace(substring, replaceWith);

		/// <inheritdoc />
		public override string ToString() => underlyingValue;

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(NonEmptyString other) => underlyingValue == other.underlyingValue;

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is NonEmptyString other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode() => underlyingValue.GetHashCode();

		#endregion

		/// <summary>
		/// Implicit cast operator '<see cref="string"/> -> <see cref="NonEmptyString"/>'.
		/// </summary>
		public static implicit operator NonEmptyString(string stringValue) => new(stringValue);

		/// <summary>
		/// Implicit cast operator '<see cref="NonEmptyString"/> -> <see cref="string"/>'.
		/// </summary>
		public static implicit operator string(NonEmptyString nonEmptyString)
			=> nonEmptyString.underlyingValue
			   ?? throw new ArgumentException(
				   $"Default value of '{nameof(NonEmptyString)}' " +
				   $"can't be casted to '{nameof(String)}' type.");
	}
}