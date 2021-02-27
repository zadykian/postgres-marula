using Microsoft.Extensions.Configuration;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.SolutionComponents;

namespace Postgres.Marula.Infrastructure.Configuration
{
	/// <summary>
	/// Base class for application component configuration.
	/// </summary>
	public abstract class ConfigurationBase<TComponent>
		where TComponent : ISolutionComponent
	{
		protected ConfigurationBase(IConfiguration configuration)
			=> ConfigurationSection = typeof(TComponent)
				.Name
				.Replace("SolutionComponent", string.Empty)
				.To(configuration.GetSection);

		/// <summary>
		/// Configuration section of current solution component. 
		/// </summary>
		protected IConfigurationSection ConfigurationSection { get; }
	}
}