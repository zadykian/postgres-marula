using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.AppControl.UIElements.Menu;
using Terminal.Gui;

namespace Postgres.Marula.AppControl.UIElements
{
	/// <summary>
	/// Main UI window.
	/// </summary>
	internal class ControlWindow : Window
	{
		private readonly IAppMenu appMenu;

		public ControlWindow(IAppMenu appMenu) => this.appMenu = appMenu;

		/// <summary>
		/// Perform main window initialization.
		/// </summary>
		public async Task InitializeAsync()
		{
			await Task.CompletedTask;
			Title = "postgres-marula-ctl";

			Width = Dim.Fill();
			Height = Dim.Fill();
			ColorScheme = new ColorScheme
			{
				Normal = Application.Driver.MakeAttribute(fore: Color.White, back: Color.DarkGray),
				Focus = Application.Driver.MakeAttribute(fore: Color.White, back: Color.Cyan),
				HotNormal = Application.Driver.MakeAttribute(fore: Color.White, back: Color.DarkGray),
				HotFocus = Application.Driver.MakeAttribute(fore: Color.White, back: Color.DarkGray),
			};

			//ColorScheme = Colors.Base;

			var generalMenuItems = appMenu
				.LoadGeneral()
				.ToArray();

			var jobMenuItems = await appMenu
				.LoadJobsAsync()
				.ToArrayAsync();

			var menuWidth = generalMenuItems
				.Concat(jobMenuItems)
				.Max(item => item.Name.Length) + 5;

			var generalMenuView = new FrameView("general")
			{
				Width = menuWidth,
				Height = Dim.Percent(50f),
				CanFocus = false
			};

			generalMenuView.ShortcutAction = () => generalMenuView.SetFocus();

			var generalMenuListView = new ListView(generalMenuItems)
			{
				Width = Dim.Fill(),
				Height = Dim.Fill(),
				AllowsMarking = false,
				CanFocus = true
			};

			generalMenuListView.OpenSelectedItem += eventArgs =>
			{
				// todo
				var selectedValue = eventArgs.Value;
			};
			
			generalMenuView.Add(generalMenuListView);

			Add(generalMenuView);

			var jobsMenuView = new FrameView("jobs")
			{
				Y = Pos.Bottom(generalMenuView),
				Width = menuWidth,
				Height = Dim.Percent(50f),
				CanFocus = false
			};

			jobsMenuView.ShortcutAction = () => jobsMenuView.SetFocus();

			jobsMenuView.Add(new ListView(jobMenuItems)
			{
				Width = Dim.Fill(),
				Height = Dim.Fill(),
				AllowsMarking = false,
				CanFocus = true,
			});
			
			Add(jobsMenuView);
		}
	}
}