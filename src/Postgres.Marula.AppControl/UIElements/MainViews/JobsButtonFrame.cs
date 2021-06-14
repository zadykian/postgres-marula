using Postgres.Marula.AppControl.UIElements.Controls;
using Postgres.Marula.Infrastructure.Extensions;
using Terminal.Gui;

namespace Postgres.Marula.AppControl.UIElements.MainViews
{
	/// <summary>
	/// Frame with job management buttons. 
	/// </summary>
	internal class JobButtonsFrame : FrameView
	{
		private readonly IButtons buttons;

		public JobButtonsFrame(IButtons buttons) => this.buttons = buttons;

		/// <summary>
		/// Perform initialization. 
		/// </summary>
		public JobButtonsFrame Initialize()
		{
			var startAllButton = buttons.StartAllJobs();
			Height = Dim.Height(startAllButton) + 2;
			Width = Dim.Fill();

			Add(startAllButton);

			buttons
				.StopAllJobs()
				.WithHorizontalOffset(Pos.Right(startAllButton) + 1)
				.To(Add);

			return this;
		}
	}
}