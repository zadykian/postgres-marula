using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Infrastructure.AppComponents;
using Postgres.Marula.Infrastructure.JsonSerialization;

[assembly: InternalsVisibleTo("Postgres.Marula.Tests")]

namespace Postgres.Marula.Infrastructure
{
	/// <inheritdoc />
	public class InfrastructureAppComponent : IAppComponent
	{
		/// <inheritdoc />
		IServiceCollection IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection.AddSingleton<IJsonConverters, JsonConverters>();
	}
}