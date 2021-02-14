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

		/// <summary>
		/// Get parameters recalculation interval. 
		/// </summary>
		PositiveTimeSpan GetRecalculationInterval();

		/// <summary>
		/// Automatic database's parameters adjustment is enabled. 
		/// </summary>
		bool AutoAdjustIsEnabled();
	}
}