using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.Menu.Items;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.MainViews
{
	/// <summary>
	/// Host's jobs menu.
	/// </summary>
	internal class JobsMenu : FrameView
	{
		private readonly IAppMenu appMenu;
		private readonly JobButtonsFrame jobButtonsFrame;

		public JobsMenu(IAppMenu appMenu, JobButtonsFrame jobButtonsFrame)
		{
			this.appMenu = appMenu;
			this.jobButtonsFrame = jobButtonsFrame;
		}

		/// <summary>
		/// Perform initialization. 
		/// </summary>
		public async Task<JobsMenu> InitializeAsync(Pos verticalOffset)
		{
			Title = "jobs";
			Y = verticalOffset;
			Width = await appMenu.TotalWidthAsync().ConfigureAwait(false); // todo;
			Height = Dim.Fill();
			CanFocus = false;

			ShortcutAction = SetFocus;

			Add(jobButtonsFrame.Initialize());

			var jobMenuItems = await appMenu
				.LoadJobsAsync()
				.ToArrayAsync()
				.ConfigureAwait(false); // todo;

			Add(new ListView(jobMenuItems)
			{
				Y = Pos.Bottom(jobButtonsFrame),
				Width = Dim.Fill(),
				Height = Dim.Fill(),
				AllowsMarking = false,
				CanFocus = true
			});

			return this;
		}
	}
}