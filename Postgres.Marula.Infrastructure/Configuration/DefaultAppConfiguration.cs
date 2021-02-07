using Microsoft.Extensions.Configuration;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Infrastructure.Configuration
{
	/// <inheritdoc />
	internal class DefaultAppConfiguration : IAppConfiguration
	{
		private readonly IConfiguration configuration;

		public DefaultAppConfiguration(IConfiguration configuration) => this.configuration = configuration;

		/// <inheritdoc />
		ConnectionString IAppConfiguration.GetConnectionString()
			=> configuration
				.GetConnectionString("Default")
				.To(connectionString => ConnectionString.Parse(connectionString));
	}
}