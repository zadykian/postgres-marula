using Postgres.Marula.AppControl.UIElements.Messages;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;
using Terminal.Gui;

namespace Postgres.Marula.AppControl.UIElements.Controls
{
	/// <inheritdoc />
	internal class Buttons : IButtons
	{
		private readonly IJobs jobs;
		private readonly IMessageBox messageBox;

		public Buttons(IJobs jobs, IMessageBox messageBox)
		{
			this.jobs = jobs;
			this.messageBox = messageBox;
		}

		/// <inheritdoc />
		Button IButtons.StartAllJobs()
		{
			var startAllButton = new Button("start all")
			{
				ColorScheme = ButtonColorScheme()
			};

			startAllButton.Clicked += async () =>
				await messageBox
					.QueryAsync("start jobs", "send request to host app?")
					.OnConfirmed(jobs.StartAllAsync);

			return startAllButton;
		}

		/// <inheritdoc />
		Button IButtons.StopAllJobs()
		{
			var stopAllButton = new Button("stop all")
			{
				ColorScheme = ButtonColorScheme()
			};

			stopAllButton.Clicked += async () =>
				await messageBox
					.QueryAsync("stop jobs", "send request to host app?")
					.OnConfirmed(jobs.StopAllAsync);

			return stopAllButton;
		}

		/// <summary>
		/// Create UI color scheme for buttons. 
		/// </summary>
		private static ColorScheme ButtonColorScheme()
		{
			var normal = Application.Driver.MakeAttribute(fore: Color.Black, back: Color.Gray);
			var focused = Application.Driver.MakeAttribute(fore: Color.White, back: Color.Cyan);

			return new ColorScheme
			{
				Normal = normal,
				Focus = focused,
				HotNormal = normal,
				HotFocus = normal
			};
		}
	}
}