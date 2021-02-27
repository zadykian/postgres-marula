using Microsoft.Extensions.Configuration;
using Postgres.Marula.Infrastructure.Configuration;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.Configuration
{
	/// <inheritdoc cref="IDatabaseAccessConfiguration"/>
	internal class DefaultDatabaseAccessConfiguration :
		ConfigurationBase<DatabaseAccessSolutionComponent>,
		IDatabaseAccessConfiguration
	{
		public DefaultDatabaseAccessConfiguration(IConfiguration configuration) : base(configuration)
		{
		}

		/// <inheritdoc />
		ConnectionString IDatabaseAccessConfiguration.ConnectionString()
			=> ConfigurationSection
				.GetSection("ConnectionString")
				.To(stringSection => new ConnectionString(stringSection.Value));
	}
}