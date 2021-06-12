using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Infrastructure.AppComponents;

namespace Postgres.Marula.AppControl
{
	/// <inheritdoc />
	internal class AppControlAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection services)
			=> services
				.AddSingleton<IHostedService, TerminalUiService>();
	}
}