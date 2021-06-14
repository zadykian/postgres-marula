using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.AppControl.PeriodicJobs;
using Postgres.Marula.AppControl.UIElements;
using Postgres.Marula.AppControl.UIElements.Lifetime;
using Postgres.Marula.AppControl.UIElements.Menu;
using Postgres.Marula.AppControl.UIElements.Messages;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;
using Postgres.Marula.Infrastructure.AppComponents;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.AppControl
{
	/// <inheritdoc />
	internal class AppControlAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection services)
			=> services
				.AddSingleton<IMessageBox, TerminalMessageBox>()
				.AddSingleton<IJobs, RemoteJobs>()
				.To(RegisterGeneralMenuItems)
				.AddSingleton<IAppMenu, AppMenu>()
				.AddSingleton<IUIStartup, ControlWindow>()
				.AddSingleton<IUIShutdown, UiShutdown>()
				.AddSingleton<IHostedService, TerminalUiService>();

		private static IServiceCollection RegisterGeneralMenuItems(IServiceCollection services)
			=> services
				.AddSingleton<IMenuItem>(new MenuItem("view ctl app logs", 0))
				.AddSingleton<IMenuItem>(new MenuItem("calculate immediately", 1))
				.AddSingleton<IMenuItem>(new MenuItem("view calculated values", 2))
				.AddSingleton<IMenuItem>(new MenuItem("export values to .sql", 3))
				.AddSingleton<IMenuItem, QuitMenuItem>();
	}
}