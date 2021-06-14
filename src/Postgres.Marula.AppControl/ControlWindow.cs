using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.AppControl.UIElements;
using Postgres.Marula.AppControl.UIElements.Menu;
using Terminal.Gui;

namespace Postgres.Marula.AppControl
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
				Normal = Application.Driver.MakeAttribute(fore: Color.White, back: Color.DarkGray)
			};
			
			var menuView = new FrameView ("app menu") 
			{
				// X = 0,
				// Y = 0,
				Width = 25,
				Height = Dim.Fill (1),
				CanFocus = false,
				Shortcut = Key.CtrlMask | Key.C
			};
			
			menuView.Title = $"{menuView.Title} ({menuView.ShortcutTag})";
			menuView.ShortcutAction = () => menuView.SetFocus();

			var menuItems = await appMenu
				.LoadAsync()
				.ToArrayAsync();
			
			var menuItemsView = new ListView(menuItems)
			{
				X = 0,
				Y = 0,
				Width = Dim.Fill(),
				Height = Dim.Fill(),
				AllowsMarking = false,
				CanFocus = true,
			};

			menuView.Add(menuItemsView);
		}
	}
}