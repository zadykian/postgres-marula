using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Database connection string represented as 'key1=value1; key2=value2; ...'.
	/// </summary>
	public class ConnectionString
	{
		private readonly IReadOnlyDictionary<NonEmptyString, NonEmptyString> parameters;

		public ConnectionString(NonEmptyString connectionString) => parameters = Parse(connectionString);

		/// <inheritdoc/>
		public override string ToString()
			=> parameters
				.Select(pair => $"{pair.Key}={pair.Value}")
				.JoinBy("; ");

		/// <summary>
		/// Implicit cast operator <see cref="ConnectionString"/> -> <see cref="string"/>.
		/// </summary>
		public static implicit operator string(ConnectionString connectionString) => connectionString.ToString();

		/// <summary>
		/// Implicit cast operator <see cref="string"/> -> <see cref="ConnectionString"/>.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Parameter <paramref name="connectionString"/> does not match pattern 'key1=value1; key2=value2; ...'.
		/// </exception>
		public static implicit operator ConnectionString(string connectionString) => new (connectionString);

		/// <summary>
		/// Parse input string <paramref name="stringToParse"/> into <see cref="ConnectionString"/> instance.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Parameter <paramref name="stringToParse"/> does not match pattern 'key1=value1; key2=value2; ...'.
		/// </exception>
		private static IReadOnlyDictionary<NonEmptyString, NonEmptyString> Parse(NonEmptyString stringToParse)
		{
			KeyValuePair<NonEmptyString, NonEmptyString> ParseSingleKeyValue(string stringToken)
				=> stringToken
					.Split(separator: '=')
					.Select(token => token.Trim())
					.ToImmutableArray()
					.ThrowIf(
						array => array.Length != 2,
						() => new ArgumentException($"Invalid connection string: '{stringToParse}'.", nameof(stringToParse)))
					.To(array => new KeyValuePair<NonEmptyString, NonEmptyString>(array.First().ToLower(), array.Last()));

			return ((string) stringToParse)
				.Split(separator: ';')
				.Where(token => !string.IsNullOrWhiteSpace(token))
				.Select(ParseSingleKeyValue)
				.ToImmutableDictionary();
		}
	}
}