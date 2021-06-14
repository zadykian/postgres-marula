using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.AppControl.UIElements.Menu;
using Terminal.Gui;

namespace Postgres.Marula.AppControl.UIElements.MainViews
{
	/// <summary>
	/// Main host's jobs menu.
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
			Width = await appMenu.TotalWidthAsync();
			Height = Dim.Fill();
			CanFocus = false;

			ShortcutAction = SetFocus;

			Add(jobButtonsFrame.Initialize());

			var jobMenuItems = await appMenu
				.LoadJobsAsync()
				.ToArrayAsync();

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