using Microsoft.Extensions.DependencyInjection;

namespace Postgres.Marula.Infrastructure
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
		void Install(IServiceCollection serviceCollection);
	}
}