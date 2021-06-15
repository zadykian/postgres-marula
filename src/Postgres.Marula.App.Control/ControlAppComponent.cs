using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.App.Control.Configuration;
using Postgres.Marula.App.Control.PeriodicJobs;
using Postgres.Marula.App.Control.UIElements.Controls;
using Postgres.Marula.App.Control.UIElements.Lifetime;
using Postgres.Marula.App.Control.UIElements.MainViews;
using Postgres.Marula.App.Control.UIElements.Menu;
using Postgres.Marula.App.Control.UIElements.Messages;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;
using Postgres.Marula.Infrastructure.AppComponents;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.App.Control
{
	/// <inheritdoc />
	internal class ControlAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection services)
			=> services
				.AddSingleton<IControlAppConfiguration, ControlAppConfiguration>()
				.AddSingleton<IJobs, RemoteJobs>()
				.AddSingleton<IAppMenu, AppMenu>()
				.To(RegisterUIElements)
				.AddSingleton<IHostedService, TerminalUiService>();

		/// <summary>
		/// Add all UI elements to <paramref name="services"/>. 
		/// </summary>
		private static IServiceCollection RegisterUIElements(IServiceCollection services)
			=> services
				.AddSingleton<IMessageBox, TerminalMessageBox>()
				.AddSingleton<IButtons, Buttons>()
				.AddSingleton<JobButtonsFrame>()
				.AddSingleton<JobsMenu>()
				.To(RegisterGeneralMenuItems)
				.AddSingleton<GeneralMenu>()
				.AddSingleton<IUIStartup, MainWindow>()
				.AddSingleton<IUIShutdown, UiShutdown>();

		/// <summary>
		/// Add general menu UI elements to <paramref name="services"/>. 
		/// </summary>
		private static IServiceCollection RegisterGeneralMenuItems(IServiceCollection services)
			=> services
				.AddSingleton<IMenuItem>(new MenuItem("view ctl app logs", 0))
				.AddSingleton<IMenuItem>(new MenuItem("calculate immediately", 1))
				.AddSingleton<IMenuItem>(new MenuItem("view calculated values", 2))
				.AddSingleton<IMenuItem>(new MenuItem("export values to .sql", 3))
				.AddSingleton<IMenuItem, QuitMenuItem>();
	}
}