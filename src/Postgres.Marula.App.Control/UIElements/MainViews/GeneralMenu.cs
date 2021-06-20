using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.Menu.Items;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.MainViews
{
	/// <summary>
	/// General app menu.
	/// </summary>
	internal class GeneralMenu : FrameView
	{
		private readonly GeneralButtonFrame generalButtonFrame;
		private readonly IAppMenu appMenu;

		public GeneralMenu(
			GeneralButtonFrame generalButtonFrame,
			IAppMenu appMenu)
		{
			this.generalButtonFrame = generalButtonFrame;
			this.appMenu = appMenu;
		}

		/// <summary>
		/// Perform initialization. 
		/// </summary>
		public async Task<GeneralMenu> InitializeAsync()
		{
			Title = "general";
			Width = await appMenu.TotalWidthAsync().ConfigureAwait(false); // todo;
			Height = Dim.Percent(50f);

			CanFocus = false;
			ShortcutAction = SetFocus;

			Add(generalButtonFrame.Initialize());

			var generalMenuListView = new ListView(appMenu.LoadGeneral().ToArray())
			{
				Width = Dim.Fill(),
				Height = Dim.Fill(),
				Y = Pos.Bottom(generalButtonFrame),
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