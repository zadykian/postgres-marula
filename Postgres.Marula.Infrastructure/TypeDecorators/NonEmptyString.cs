using System;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Non-empty string.
	/// </summary>
	public readonly struct NonEmptyString : IEquatable<NonEmptyString>
	{
		private readonly string stringValue;

		private NonEmptyString(string stringValue)
			=> this.stringValue = string.IsNullOrWhiteSpace(stringValue)
				? throw new ArgumentException("String value can't be whitespace.", nameof(stringValue))
				: stringValue;

		/// <inheritdoc />
		public override string ToString() => stringValue;

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(NonEmptyString other) => stringValue == other.stringValue;

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is NonEmptyString other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode() => stringValue.GetHashCode();

		/// <summary>
		/// <see cref="NonEmptyString"/> equality operator. 
		/// </summary>
		public static bool operator ==(NonEmptyString left, NonEmptyString right) => left.Equals(right);

		/// <inheritdoc cref="op_Equality"/>
		public static bool operator !=(NonEmptyString left, NonEmptyString right) => !left.Equals(right);

		#endregion

		/// <summary>
		/// Implicit cast operator '<see cref="string"/> -> <see cref="NonEmptyString"/>'. 
		/// </summary>
		public static implicit operator NonEmptyString(string stringValue) => new(stringValue);

		/// <summary>
		/// Implicit cast operator '<see cref="NonEmptyString"/> -> <see cref="string"/>'. 
		/// </summary>
		public static implicit operator string(NonEmptyString nonEmptyString)
			=> nonEmptyString.stringValue
			   ?? throw new ArgumentException($"Default value of '{nameof(NonEmptyString)}' can't be casted to '{nameof(String)}' type.");
	}
}