using Microsoft.Extensions.Configuration;
using Postgres.Marula.Infrastructure.AppComponents;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Infrastructure.Configuration
{
	/// <summary>
	/// Base class for application component configuration.
	/// </summary>
	public abstract class ConfigurationBase<TAppComponent>
		where TAppComponent : IAppComponent
	{
		protected ConfigurationBase(IConfiguration configuration)
			=> ConfigurationSection = typeof(TAppComponent)
				.Name
				.Replace("AppComponent", string.Empty)
				.To(configuration.GetSection);

		/// <summary>
		/// Configuration section of current component. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// Value of <see cref="IConfigurationSection.Key"/> matches <typeparamref name="TAppComponent"/> type name prefix.
		/// </para>
		/// <para>
		/// Therefore, component name must have pattern '[SectionKey]AppComponent'.
		/// </para>
		/// </remarks>
		protected IConfigurationSection ConfigurationSection { get; }
	}
}