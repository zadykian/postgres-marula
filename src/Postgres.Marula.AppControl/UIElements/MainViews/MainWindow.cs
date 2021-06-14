using System.Threading.Tasks;
using Postgres.Marula.AppControl.UIElements.Lifetime;
using Terminal.Gui;

namespace Postgres.Marula.AppControl.UIElements.MainViews
{
	/// <summary>
	/// Main UI window.
	/// </summary>
	internal class MainWindow : Window, IUIStartup
	{
		private readonly GeneralMenu generalMenu;
		private readonly JobsMenu jobsMenu;

		public MainWindow(GeneralMenu generalMenu, JobsMenu jobsMenu)
		{
			this.jobsMenu = jobsMenu;
			this.generalMenu = generalMenu;
		}

		/// <inheritdoc />
		async Task IUIStartup.StartAsync()
		{
			Application.Init();
			await InitializeAsync().ConfigureAwait(false); // todo
			Application.Run(this);
		}

		/// <summary>
		/// Perform main window initialization.
		/// </summary>
		private async Task InitializeAsync()
		{
			Title = "postgres-marula-ctl";
			ColorScheme = DefaultColorScheme();
			Width = Dim.Fill();
			Height = Dim.Fill();

			Add(await generalMenu.InitializeAsync().ConfigureAwait(false)); // todo;
			Add(await jobsMenu.InitializeAsync(Pos.Bottom(generalMenu)));

			var currentOutputView = new FrameView("current output")
			{
				X = Pos.Right(generalMenu),
				Width = Dim.Fill(),
				Height = Dim.Fill(),
				CanFocus = false
			};

			Add(currentOutputView);
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