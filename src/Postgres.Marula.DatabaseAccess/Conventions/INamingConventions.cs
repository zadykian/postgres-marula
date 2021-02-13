using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.Conventions
{
	/// <summary>
	/// Database objects' naming conventions.
	/// </summary>
	internal interface INamingConventions
	{
		/// <summary>
		/// Name of database schema that contains all system tables, types and so on.
		/// </summary>
		DatabaseObjectName SystemSchemaName { get; }

		/// <summary>
		/// Name of calculated parameter values table.
		/// </summary>
		DatabaseObjectName ValuesHistoryTableName { get; } 
	}
}