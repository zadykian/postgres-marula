using Postgres.Marula.App.Control.UIElements.Buttons;
using Postgres.Marula.App.Control.UIElements.Extensions;
using Postgres.Marula.Infrastructure.Extensions;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.MainViews.Jobs
{
	/// <summary>
	/// Frame with job management buttons. 
	/// </summary>
	internal class JobButtonsFrame : FrameView
	{
		private readonly StartJobsButton startJobsButton;
		private readonly StopJobsButton stopJobsButton;

		public JobButtonsFrame(
			StartJobsButton startJobsButton,
			StopJobsButton stopJobsButton)
		{
			this.startJobsButton = startJobsButton;
			this.stopJobsButton = stopJobsButton;
		}

		/// <summary>
		/// Perform initialization. 
		/// </summary>
		public JobButtonsFrame Initialize()
		{
			Height = Dim.Height(startJobsButton.Initialize()) + 2;
			Width = Dim.Fill();

			Add(startJobsButton);

			stopJobsButton
				.Initialize()
				.WithHorizontalOffset(Pos.Right(startJobsButton) + 1)
				.To(Add);

			return this;
		}
	}
}