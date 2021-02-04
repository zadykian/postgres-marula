namespace Postgres.Marula.DatabaseAccess.Conventions
{
	/// <inheritdoc />
	internal class DefaultNamingConventions : INamingConventions
	{
		/// <inheritdoc />
		string INamingConventions.SystemSchemaName => "pg_marula";
	}
}