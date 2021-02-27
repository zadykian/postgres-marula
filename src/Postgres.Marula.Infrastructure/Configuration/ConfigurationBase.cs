using Microsoft.Extensions.Configuration;
using Postgres.Marula.Infrastructure.AppComponents;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Infrastructure.Configuration
{
	/// <summary>
	/// Base class for application component configuration.
	/// </summary>
	public abstract class ConfigurationBase<TComponent>
		where TComponent : IAppComponent
	{
		protected ConfigurationBase(IConfiguration configuration)
			=> ConfigurationSection = typeof(TComponent)
				.Name
				.Replace("Component", string.Empty)
				.To(configuration.GetSection);

		/// <summary>
		/// Configuration section of current component. 
		/// </summary>
		protected IConfigurationSection ConfigurationSection { get; }
	}
}