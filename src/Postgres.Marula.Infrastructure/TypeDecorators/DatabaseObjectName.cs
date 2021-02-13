using System;
using System.Text.RegularExpressions;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Valid Postgres object name.
	/// </summary>
	public readonly struct DatabaseObjectName : IEquatable<DatabaseObjectName>
	{
		private readonly NonEmptyString objectName;

		public DatabaseObjectName(NonEmptyString objectName)
			=> this.objectName = Regex.IsMatch(objectName, @"^[a-z|A-Z|_]{1}[a-z|A-Z|_|\d]{1,62}$")
				? objectName
				: throw new ArgumentException($"Value '{objectName}' must be valid PG object name.", nameof(objectName));

		/// <inheritdoc />
		public override string ToString() => objectName;

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(DatabaseObjectName other) => objectName.Equals(other.objectName);

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is DatabaseObjectName other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode() => objectName.GetHashCode();

		/// <summary>
		/// <see cref="DatabaseObjectName"/> equality operator. 
		/// </summary>
		public static bool operator ==(DatabaseObjectName left, DatabaseObjectName right) => left.Equals(right);

		/// <inheritdoc cref="op_Equality"/>
		public static bool operator !=(DatabaseObjectName left, DatabaseObjectName right) => !left.Equals(right);

		#endregion

		/// <summary>
		/// Implicit cast operator '<see cref="string"/> -> <see cref="DatabaseObjectName"/>'. 
		/// </summary>
		public static implicit operator DatabaseObjectName(string stringValue) => new(stringValue);

		/// <summary>
		/// Implicit cast operator '<see cref="DatabaseObjectName"/> -> <see cref="string"/>'. 
		/// </summary>
		public static implicit operator string(DatabaseObjectName databaseObjectName) => databaseObjectName.objectName;
	}
}