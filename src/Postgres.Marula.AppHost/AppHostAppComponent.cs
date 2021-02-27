using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Infrastructure.AppComponents;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.AppHost
{
	/// <inheritdoc />
	internal class AppHostAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection.AddHostedService<HostedService>();
	}
}