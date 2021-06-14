using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.AppControl.UIElements.Controls;
using Postgres.Marula.AppControl.UIElements.Lifetime;
using Postgres.Marula.AppControl.UIElements.Menu;
using Postgres.Marula.Infrastructure.Extensions;
using Terminal.Gui;

namespace Postgres.Marula.AppControl.UIElements
{
	/// <summary>
	/// Main UI window.
	/// </summary>
	internal class ControlWindow : Window, IUIStartup
	{
		private readonly IAppMenu appMenu;
		private readonly IButtons buttons;

		public ControlWindow(IAppMenu appMenu, IButtons buttons)
		{
			this.appMenu = appMenu;
			this.buttons = buttons;
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
		public async Task InitializeAsync()
		{
			Title = "postgres-marula-ctl";
			ColorScheme = DefaultColorScheme();
			Width = Dim.Fill();
			Height = Dim.Fill();

			var generalMenuItems = appMenu
				.LoadGeneral()
				.ToArray();

			var jobMenuItems = await appMenu
				.LoadJobsAsync()
				.ToArrayAsync()
				.ConfigureAwait(false); // todo

			var menuWidth = generalMenuItems
				.Concat(jobMenuItems)
				.Max(item => item.Name.Length) + 4;

			var generalMenuView = CreateGeneralMenuView(menuWidth, generalMenuItems);
			Add(generalMenuView);

			var jobsMenuView = CreateJobsMenuView(menuWidth, jobMenuItems, generalMenuView);
			Add(jobsMenuView);

			var currentOutputView = new FrameView("current output")
			{
				X = Pos.Right(generalMenuView),
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

		/// <summary>
		/// Create general menu view. 
		/// </summary>
		private static FrameView CreateGeneralMenuView(
			int menuWidth,
			IEnumerable<IMenuItem> generalMenuItems)
		{
			var generalMenuView = new FrameView("general")
			{
				Width = menuWidth,
				Height = Dim.Percent(50f),
				CanFocus = false
			};

			generalMenuView.ShortcutAction = () => generalMenuView.SetFocus();

			var generalMenuListView = new ListView(generalMenuItems.ToArray())
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

			generalMenuView.Add(generalMenuListView);
			return generalMenuView;
		}

		/// <summary>
		/// Create main host's jobs view. 
		/// </summary>
		private FrameView CreateJobsMenuView(
			int menuWidth,
			IEnumerable<IMenuItem> jobMenuItems,
			View generalMenuView)
		{
			var jobsMenuView = new FrameView("jobs")
			{
				Y = Pos.Bottom(generalMenuView),
				Width = menuWidth,
				Height = Dim.Fill(),
				CanFocus = false
			};

			jobsMenuView.ShortcutAction = () => jobsMenuView.SetFocus();

			var buttonsFrame = CreateButtonsFrame();
			jobsMenuView.Add(buttonsFrame);

			jobsMenuView.Add(new ListView(jobMenuItems.ToArray())
			{
				Y = Pos.Bottom(buttonsFrame),
				Width = Dim.Fill(),
				Height = Dim.Fill(),
				AllowsMarking = false,
				CanFocus = true
			});

			return jobsMenuView;
		}

		/// <summary>
		/// Create frame with job management buttons. 
		/// </summary>
		private FrameView CreateButtonsFrame()
		{
			var startAllButton = buttons.StartAllJobs();

			var buttonsFrame = new FrameView
			{
				Height = Dim.Height(startAllButton) + 2,
				Width = Dim.Fill()
			};

			buttonsFrame.Add(startAllButton);

			buttons
				.StopAllJobs()
				.WithHorizontalOffset(Pos.Right(startAllButton) + 1)
				.To(buttonsFrame.Add);

			return buttonsFrame;
		}
	}
}