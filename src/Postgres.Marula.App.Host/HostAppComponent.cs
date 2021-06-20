using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Infrastructure.AppComponents;

namespace Postgres.Marula.App.Host
{
	/// <inheritdoc />
	internal class HostAppComponent : IAppComponent
	{
		/// <inheritdoc />
		IServiceCollection IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection.AddHostedService<JobRunner>();
	}
}