using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.App.Control.Configuration;
using Postgres.Marula.App.Control.HostAccess;
using Postgres.Marula.App.Control.UIElements.Buttons;
using Postgres.Marula.App.Control.UIElements.Lifetime;
using Postgres.Marula.App.Control.UIElements.MainViews;
using Postgres.Marula.App.Control.UIElements.MainViews.General;
using Postgres.Marula.App.Control.UIElements.MainViews.Jobs;
using Postgres.Marula.App.Control.UIElements.MainViews.Loader;
using Postgres.Marula.App.Control.UIElements.MainViews.Output;
using Postgres.Marula.App.Control.UIElements.Menu;
using Postgres.Marula.App.Control.UIElements.Menu.Items;
using Postgres.Marula.App.Control.UIElements.Menu.Items.Base;
using Postgres.Marula.App.Control.UIElements.Messages;
using Postgres.Marula.App.Control.ValuesExport;
using Postgres.Marula.Calculations.PublicApi;
using Postgres.Marula.Infrastructure.AppComponents;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.App.Control
{
	/// <inheritdoc />
	internal class ControlAppComponent : IAppComponent
	{
		/// <inheritdoc />
		IServiceCollection IAppComponent.RegisterServices(IServiceCollection services)
			=> services
				.AddSingleton<IControlAppConfiguration, ControlAppConfiguration>()
				.AddSingleton<IJobs, RemoteJobs>()
				.AddSingleton<IParameterValues, RemoteParameterValues>()
				.AddSingleton<IValuesExport, LocalValuesExport>()
				.AddSingleton<IAppMenu, AppMenu>()
				.To(RegisterUIElements)
				.AddSingleton<IHostedService, TerminalUiService>();

		/// <summary>
		/// Add all UI elements to <paramref name="services"/>. 
		/// </summary>
		private static IServiceCollection RegisterUIElements(IServiceCollection services)
			=> services
				.AddSingleton<IMessageBox, TerminalMessageBox>()
				.To(AddGeneralMenu)
				.To(AddJobsMenu)
				.AddSingleton<OutputWindow>()
				.Forward<OutputWindow, IOutputWindow>()
				.AddSingleton<LoadingBar>()
				.Forward<LoadingBar, ILoader>()
				.AddSingleton<IUIStartup, MainWindow>()
				.AddSingleton<IUIShutdown, UiShutdown>();

		/// <summary>
		/// Add general menu and its' nested elements to <paramref name="services"/>. 
		/// </summary>
		private static IServiceCollection AddGeneralMenu(IServiceCollection services)
			=> services
				.AddSingleton<CalculateValuesButton>()
				.AddSingleton<ExportValuesButton>()
				.AddSingleton<ApplyValuesButton>()
				.AddSingleton<GeneralButtonFrame>()
				.AddSingleton<IMenuItem, AppLogsMenuItem>()
				.AddSingleton<IMenuItem, CalculatedValuesMenuItem>()
				.AddSingleton<IMenuItem, QuitMenuItem>()
				.AddSingleton<GeneralMenu>();

		/// <summary>
		/// Add jobs menu and its' nested elements to <paramref name="services"/>. 
		/// </summary>
		private static IServiceCollection AddJobsMenu(IServiceCollection services)
			=> services
				.AddSingleton<StartJobsButton>()
				.AddSingleton<StopJobsButton>()
				.AddSingleton<JobButtonsFrame>()
				.AddSingleton<JobsMenu>();
	}
}