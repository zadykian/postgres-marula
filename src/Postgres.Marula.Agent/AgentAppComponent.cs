using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Infrastructure.AppComponents;

namespace Postgres.Marula.Agent
{
	/// <inheritdoc />
	internal class AgentAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<IStartup, AgentStartup>();
	}
}