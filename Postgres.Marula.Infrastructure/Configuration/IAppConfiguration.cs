using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Infrastructure.Configuration
{
	/// <summary>
	/// Application configuration.
	/// </summary>
	public interface IAppConfiguration
	{
		/// <summary>
		/// Get database connection string.
		/// </summary>
		ConnectionString GetConnectionString();
	}
}