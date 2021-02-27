using Microsoft.Extensions.Configuration;

namespace Postgres.Marula.Infrastructure.Configuration
{
	/// <summary>
	/// Base class for application component configuration.
	/// </summary>
	public abstract class ConfigurationBase
	{
		protected ConfigurationBase(IConfiguration configuration) => Configuration = configuration;

		/// <inheritdoc cref="IConfiguration"/>
		protected IConfiguration Configuration { get; }
	}
}