using Postgres.Marula.App.Control.UIElements.Controls;
using Postgres.Marula.Infrastructure.Extensions;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.MainViews
{
	/// <summary>
	/// Frame with general management buttons. 
	/// </summary>
	internal class GeneralButtonFrame : FrameView
	{
		private readonly IButtons buttons;

		public GeneralButtonFrame(IButtons buttons) => this.buttons = buttons;

		/// <summary>
		/// Perform initialization. 
		/// </summary>
		public GeneralButtonFrame Initialize()
		{
			var calculateButton = buttons.CalculateImmediately();
			var exportButton = buttons.ExportValues();

			Height = Dim.Height(calculateButton) + Dim.Height(exportButton) + 3;
			Width = Dim.Fill();

			Add(calculateButton);

			exportButton
				.WithVerticalOffset(Pos.Bottom(calculateButton) + 1)
				.To(Add);

			return this;
		}
	}
}