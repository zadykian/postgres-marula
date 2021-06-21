using Postgres.Marula.App.Control.UIElements.Messages;
using Postgres.Marula.Calculations.PublicApi;

namespace Postgres.Marula.App.Control.UIElements.Buttons
{
	/// <summary>
	/// Button to stop all main host's jobs. 
	/// </summary>
	internal class StopJobsButton : ButtonBase
	{
		private readonly IJobs jobs;
		private readonly IMessageBox messageBox;

		public StopJobsButton(IJobs jobs, IMessageBox messageBox)
		{
			this.jobs = jobs;
			this.messageBox = messageBox;
		}

		/// <summary>
		/// Perform initialization.
		/// </summary>
		public StopJobsButton Initialize()
		{
			Text = "stop all";
			ColorScheme = ButtonColorScheme();

			Clicked += async ()
				=> await messageBox
					.QueryAsync("stop jobs", "send request to host app?")
					.OnConfirmed(jobs.StopAllAsync);

			return this;
		}
	}
}