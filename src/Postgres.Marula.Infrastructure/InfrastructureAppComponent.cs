using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Infrastructure.AppComponents;

[assembly: InternalsVisibleTo("Postgres.Marula.Tests")]

namespace Postgres.Marula.Infrastructure
{
	/// <inheritdoc />
	public class InfrastructureAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
		{
		}
	}
}