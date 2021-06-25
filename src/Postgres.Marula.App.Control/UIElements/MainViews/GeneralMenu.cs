using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.Extensions;
using Postgres.Marula.App.Control.UIElements.Menu;
using Postgres.Marula.App.Control.UIElements.Menu.Items.Base;
using Postgres.Marula.Infrastructure.Extensions;
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

			Width = await appMenu.TotalWidthAsync().ConfigureAwait(false);
			Height = Dim.Percent(50f);

			Add(generalButtonFrame.Initialize());

			appMenu
				.LoadGeneral()
				.AsListView()
				.FillDimensions()
				.WithVerticalOffset(Pos.Bottom(generalButtonFrame))
				.OnSelectionOf<IMenuItem>(menuItem => menuItem.HandleClickAsync())
				.To(Add);

			return this;
		}
	}
}