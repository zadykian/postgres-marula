using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.Extensions;
using Postgres.Marula.App.Control.UIElements.Menu;
using Postgres.Marula.Infrastructure.Extensions;
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
			Width = await appMenu.TotalWidthAsync().ConfigureAwait(false);
			Height = Dim.Fill();

			Add(jobButtonsFrame.Initialize());

			var jobMenuItems = await appMenu
				.LoadJobsAsync()
				.ToArrayAsync()
				.ConfigureAwait(false);

			jobMenuItems
				.AsListView()
				.FillDimensions()
				.WithVerticalOffset(Pos.Bottom(jobButtonsFrame))
				.To(Add);

			return this;
		}
	}
}