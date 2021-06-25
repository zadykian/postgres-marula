using Postgres.Marula.App.Control.UIElements.Buttons;
using Postgres.Marula.App.Control.UIElements.Extensions;
using Postgres.Marula.Infrastructure.Extensions;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.MainViews.General
{
	/// <summary>
	/// Frame with general management buttons. 
	/// </summary>
	internal class GeneralButtonFrame : FrameView
	{
		private readonly CalculateValuesButton calculateButton;
		private readonly ExportValuesButton exportButton;
		private readonly ApplyValuesButton applyButton;

		public GeneralButtonFrame(
			CalculateValuesButton calculateButton,
			ExportValuesButton exportButton,
			ApplyValuesButton applyButton)
		{
			this.calculateButton = calculateButton;
			this.exportButton = exportButton;
			this.applyButton = applyButton;
		}

		/// <summary>
		/// Perform initialization. 
		/// </summary>
		public GeneralButtonFrame Initialize()
		{
			Height = Dim.Height(calculateButton.Initialize())
			         + Dim.Height(exportButton.Initialize())
			         + Dim.Height(applyButton.Initialize())
			         + 4;

			Width = Dim.Fill();

			Add(calculateButton);

			exportButton
				.WithVerticalOffset(Pos.Bottom(calculateButton) + 1)
				.To(Add);

			applyButton
				.WithVerticalOffset(Pos.Bottom(exportButton) + 1)
				.To(Add);

			return this;
		}
	}
}