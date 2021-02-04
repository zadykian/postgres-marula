namespace Postgres.Marula.DatabaseAccess2.Conventions
{
	/// <inheritdoc />
	internal class DefaultNamingConventions : INamingConventions
	{
		/// <inheritdoc />
		string INamingConventions.SystemSchemaName => "pg_marula";
	}
}