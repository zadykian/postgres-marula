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
		string SystemSchemaName { get; }
	}
}