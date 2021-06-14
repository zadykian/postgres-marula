using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.AppControl.UIElements.Menu;
using Terminal.Gui;

namespace Postgres.Marula.AppControl.UIElements.MainViews
{
	/// <summary>
	/// General app menu.
	/// </summary>
	internal class GeneralMenu : FrameView
	{
		private readonly IAppMenu appMenu;

		public GeneralMenu(IAppMenu appMenu) => this.appMenu = appMenu;

		/// <summary>
		/// Perform initialization. 
		/// </summary>
		public async Task<GeneralMenu> InitializeAsync()
		{
			Title = "general";
			Width = await appMenu.TotalWidthAsync();
			Height = Dim.Percent(50f);

			CanFocus = false;
			ShortcutAction = SetFocus;

			var generalMenuListView = new ListView(appMenu.LoadGeneral().ToArray())
			{
				Width = Dim.Fill(),
				Height = Dim.Fill(),
				AllowsMarking = false,
				CanFocus = true
			};

			generalMenuListView.OpenSelectedItem += async eventArgs =>
			{
				var menuItem = (IMenuItem) eventArgs.Value;
				await menuItem.HandleClickAsync();
			};

			Add(generalMenuListView);
			return this;
		}
	}
}