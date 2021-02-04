using Microsoft.Extensions.DependencyInjection;

namespace Postgres.Marula.Infrastructure.SolutionComponents
{
	/// <summary>
	/// Component of solution which contains application services.
	/// </summary>
	public interface ISolutionComponent
	{
		/// <summary>
		/// Install component's services.
		/// </summary>
		/// <param name="serviceCollection">
		/// Collection of application services.
		/// </param>
		void RegisterServices(IServiceCollection serviceCollection);
	}
}