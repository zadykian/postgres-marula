using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.Extensions;
using Postgres.Marula.App.Control.UIElements.Lifetime;
using Postgres.Marula.Infrastructure.Extensions;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.MainViews
{
	/// <summary>
	/// Main UI window.
	/// </summary>
	internal class MainWindow : Window, IUIStartup
	{
		private readonly GeneralMenu generalMenu;
		private readonly JobsMenu jobsMenu;
		private readonly OutputWindow outputWindow;
		private readonly LoadingBar loadingBar;

		public MainWindow(
			GeneralMenu generalMenu,
			JobsMenu jobsMenu,
			OutputWindow outputWindow,
			LoadingBar loadingBar)
		{
			this.generalMenu = generalMenu;
			this.jobsMenu = jobsMenu;
			this.outputWindow = outputWindow;
			this.loadingBar = loadingBar;
		}

		/// <inheritdoc />
		async Task IUIStartup.StartAsync()
		{
			Application.Init();
			await InitializeAsync().ConfigureAwait(false);
			Application.Run(this);
		}

		/// <summary>
		/// Perform initialization.
		/// </summary>
		private async Task InitializeAsync()
		{
			Title = "postgres-marula-ctl";
			ColorScheme = DefaultColorScheme();
			Width = Dim.Fill();
			Height = Dim.Fill();

			Add(await generalMenu.InitializeAsync().ConfigureAwait(false));
			Add(await jobsMenu.InitializeAsync(Pos.Bottom(generalMenu)));

			loadingBar
				.WithHorizontalOffset(Pos.Right(generalMenu) + 1)
				.WithVerticalOffset(Pos.Bottom(this) - 3)
				.To(Add);

			outputWindow
				.Initialize()
				.WithHorizontalOffset(Pos.Right(generalMenu))
				.To(Add);
		}

		/// <summary>
		/// Create default UI color scheme. 
		/// </summary>
		private static ColorScheme DefaultColorScheme()
		{
			var normal = Application.Driver.MakeAttribute(fore: Color.White, back: Color.DarkGray);
			var focused = Application.Driver.MakeAttribute(fore: Color.White, back: Color.Cyan);

			return new ColorScheme
			{
				Normal = normal,
				Focus = focused,
				HotNormal = normal,
				HotFocus = normal
			};
		}
	}
}