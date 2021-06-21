using Postgres.Marula.App.Control.UIElements.Buttons.Base;
using Postgres.Marula.App.Control.UIElements.Messages;
using Postgres.Marula.Calculations.PublicApi;

namespace Postgres.Marula.App.Control.UIElements.Buttons
{
	/// <summary>
	/// Button to start all main host's jobs. 
	/// </summary>
	internal class StartJobsButton : ButtonBase
	{
		private readonly IJobs jobs;
		private readonly IMessageBox messageBox;

		public StartJobsButton(IJobs jobs, IMessageBox messageBox)
		{
			this.jobs = jobs;
			this.messageBox = messageBox;
		}

		/// <summary>
		/// Perform initialization.
		/// </summary>
		public StartJobsButton Initialize()
		{
			Text = "start all";
			ColorScheme = ButtonColorScheme();

			Clicked += async ()
				=> await messageBox
					.QueryAsync("start jobs", "send request to host app?")
					.OnConfirmed(async () => await jobs.StartAllAsync());

			return this;
		}
	}
}