namespace Postgres.Marula.Infrastructure.Configuration
{
	/// <summary>
	/// Application configuration.
	/// </summary>
	public interface IMarulaConfiguration
	{
		/// <summary>
		/// Database connection string.
		/// </summary>
		ConnectionString DatabaseConnectionString { get; }
	}
}