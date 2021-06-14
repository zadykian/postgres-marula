using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.AppControl.UIElements.Menu;
using Terminal.Gui;

namespace Postgres.Marula.AppControl.UIElements
{
	/// <summary>
	/// Main UI window.
	/// </summary>
	internal class ControlWindow : Window, IUserInterface
	{
		private readonly IAppMenu appMenu;
		private readonly IHostApplicationLifetime applicationLifetime;

		public ControlWindow(
			IAppMenu appMenu,
			IHostApplicationLifetime applicationLifetime)
		{
			this.appMenu = appMenu;
			this.applicationLifetime = applicationLifetime;
		}

		/// <inheritdoc />
		async Task IUserInterface.StartAsync()
		{
			Application.Init();
			await InitializeAsync();
			Application.Run(this);
		}

		/// <inheritdoc />
		async Task IUserInterface.StopAsync()
		{
			if (applicationLifetime.ApplicationStopping.IsCancellationRequested)
			{
				return;
			}

			await Task.CompletedTask;
			Application.RequestStop();
			Application.Top.Clear();
			applicationLifetime.StopApplication();
		}

		/// <summary>
		/// Perform main window initialization.
		/// </summary>
		public async Task InitializeAsync()
		{
			Title = "postgres-marula-ctl";
			ColorScheme = CreateColorScheme();
			Width = Dim.Fill();
			Height = Dim.Fill();

			var generalMenuItems = appMenu
				.LoadGeneral()
				.ToArray();

			var jobMenuItems = await appMenu
				.LoadJobsAsync()
				.ToArrayAsync();

			var menuWidth = generalMenuItems
				.Concat(jobMenuItems)
				.Max(item => item.Name.Length) + 5;

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
		/// Create UI color scheme. 
		/// </summary>
		private static ColorScheme CreateColorScheme()
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
		private FrameView CreateGeneralMenuView(
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

			generalMenuListView.OpenSelectedItem += eventArgs =>
			{
				var menuItem = (IMenuItem) eventArgs.Value;

				if (menuItem is QuitMenuItem)
				{
					var answerButtonNumber = MessageBox.Query(
						"quit",
						"are you sure are you want to quit from ctl app?", "yes", "no");
					if (answerButtonNumber == 0)
					{
						((IUserInterface) this).StopAsync();
						//applicationLifetime.StopApplication();
					}
				}
			};

			generalMenuView.Add(generalMenuListView);
			return generalMenuView;
		}

		/// <summary>
		/// Create main host's jobs view. 
		/// </summary>
		private static FrameView CreateJobsMenuView(
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

			jobsMenuView.Add(new ListView(jobMenuItems.ToArray())
			{
				Width = Dim.Fill(),
				Height = Dim.Fill(),
				AllowsMarking = false,
				CanFocus = true
			});

			return jobsMenuView;
		}
	}
}