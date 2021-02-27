using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.Configuration
{
	/// <summary>
	/// Database access component configuration.
	/// </summary>
	internal interface IDatabaseAccessConfiguration
	{
		/// <summary>
		/// Get database connection string.
		/// </summary>
		ConnectionString GetConnectionString();
	}
}