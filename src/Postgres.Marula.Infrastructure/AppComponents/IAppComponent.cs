using Microsoft.Extensions.DependencyInjection;

namespace Postgres.Marula.Infrastructure.AppComponents
{
	/// <summary>
	/// Component which contains application services.
	/// </summary>
	public interface IAppComponent
	{
		/// <summary>
		/// Install component's services.
		/// </summary>
		/// <param name="serviceCollection">
		/// Collection of application services.
		/// </param>
		IServiceCollection RegisterServices(IServiceCollection serviceCollection);
	}
}