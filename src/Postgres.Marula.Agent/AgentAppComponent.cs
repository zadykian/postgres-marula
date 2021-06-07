using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Agent.HwInfo;
using Postgres.Marula.Infrastructure.AppComponents;

namespace Postgres.Marula.Agent
{
	/// <inheritdoc />
	public class AgentAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<IHardwareInfo, BashHardwareInfo>();
	}
}