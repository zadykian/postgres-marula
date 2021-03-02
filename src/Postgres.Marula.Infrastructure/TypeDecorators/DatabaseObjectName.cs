using System;
using System.Text.RegularExpressions;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Valid Postgres object name.
	/// </summary>
	public readonly struct DatabaseObjectName : IEquatable<DatabaseObjectName>
	{
		private readonly NonEmptyString underlyingValue;

		public DatabaseObjectName(NonEmptyString underlyingValue)
			=> this.underlyingValue = Regex.IsMatch(underlyingValue, @"^[a-z|A-Z|_]{1}[a-z|A-Z|_|\d]{1,62}$")
				? underlyingValue
				: throw new ArgumentException($"Value '{underlyingValue}' must be valid PG object name.", nameof(underlyingValue));

		/// <inheritdoc />
		public override string ToString() => underlyingValue;

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(DatabaseObjectName other) => underlyingValue.Equals(other.underlyingValue);

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is DatabaseObjectName other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode() => underlyingValue.GetHashCode();

		#endregion

		/// <summary>
		/// Implicit cast operator '<see cref="string"/> -> <see cref="DatabaseObjectName"/>'.
		/// </summary>
		public static implicit operator DatabaseObjectName(string stringValue) => new(stringValue);

		/// <summary>
		/// Implicit cast operator '<see cref="DatabaseObjectName"/> -> <see cref="string"/>'.
		/// </summary>
		public static implicit operator string(DatabaseObjectName databaseObjectName) => databaseObjectName.underlyingValue;
	}
}