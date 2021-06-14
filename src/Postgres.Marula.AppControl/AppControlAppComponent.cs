using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.AppControl.UIElements;
using Postgres.Marula.AppControl.UIElements.Menu;
using Postgres.Marula.Infrastructure.AppComponents;

namespace Postgres.Marula.AppControl
{
	/// <inheritdoc />
	internal class AppControlAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection services)
			=> services
				.AddSingleton<IAppMenu, AppMenu>()
				.AddSingleton<IUserInterface, ControlWindow>()
				.AddSingleton<IHostedService, TerminalUiService>();
	}
}