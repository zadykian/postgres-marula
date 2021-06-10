using System.Linq;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Database object name qualified with schema which it belongs to.
	/// </summary>
	public readonly struct SchemaQualifiedObjectName
	{
		public SchemaQualifiedObjectName(
			DatabaseObjectName schema,
			DatabaseObjectName objectName)
		{
			Schema = schema;
			ObjectName = objectName;
		}

		/// <summary>
		/// Object's schema.
		/// </summary>
		public DatabaseObjectName Schema { get; }

		/// <summary>
		/// Object's name.
		/// </summary>
		public DatabaseObjectName ObjectName { get; }

		/// <inheritdoc />
		public override string ToString() => $"{Schema}.{ObjectName}";

		/// <summary>
		/// Parse <paramref name="stringToParse"/> to <see cref="SchemaQualifiedObjectName"/> instance. 
		/// </summary>
		public static SchemaQualifiedObjectName Parse(string stringToParse)
			=> stringToParse
				.Split('.')
				.To(tokens => new SchemaQualifiedObjectName(tokens.First(), tokens.Last()));

		/// <summary>
		/// Implicit cast operator '<see cref="string"/> -> <see cref="SchemaQualifiedObjectName"/>'.
		/// </summary>
		public static implicit operator SchemaQualifiedObjectName(string stringValue) => Parse(stringValue);
	}
}